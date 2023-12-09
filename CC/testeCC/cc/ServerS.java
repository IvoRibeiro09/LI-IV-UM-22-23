import java.io.*;
import java.net.*;
import java.util.Objects;
import java.util.Scanner;

public class ServerS {
    private String dominio;
    private String db;
    private String sp;
    private String dd;
    private String lg;
    private String lgall;
    private String st;
    private DatagramSocket dsocket;
    private byte[] buffer = new byte[550];
    //construtor inicial
    public ServerS(DatagramSocket dsocket){
        this.dominio="";
        this.db = "";
        this.sp = "";
        this.dd = "";
        this.lg = "";
        this.lgall = "";
        this.st = "";
        this.dsocket = dsocket;
    }
    //seters
    public void setDominio(String s){this.dominio=s;}
    public void setdb(String s){
        this.db=s;
    }
    public void setsp(String s){
        this.sp=s;
    }
    public void setdd(String s){
        this.dd=s;
    }
    public void setlg(String s){
        this.lg=s;
    }
    public void setlgall(String s){
        this.lgall=s;
    }
    public void setst(String s){
        this.st=s;
    }

    //geters
    public String getDb(){
        return this.db;
    }
    public String getSP(){
        return this.sp;
    }
    public String getDominio(){return this.dominio;}


    public void ParserSs(String str) throws IOException {  //funçao que le o ficheiro de config do servidor secundario
        Logs log = new Logs();
        try {
            File ficheiro = new File(str);
            Scanner myReader = new Scanner(ficheiro);
            while (myReader.hasNextLine()) {
                String data = myReader.nextLine();
                String[] linha = data.split(" ");
                if(Objects.equals(linha[1], "DB")) {setdb(linha[2]);setDominio(linha[0]);}
                else if(Objects.equals(linha[1], "SP")) {setsp(linha[2]);}
                else if(Objects.equals(linha[1], "DD")) {setdd(linha[2]);}
                else if(Objects.equals(linha[0], "all") && Objects.equals(linha[1], "LG")) {setlg(linha[2]);}
                else if(!Objects.equals(linha[0], "all") && Objects.equals(linha[1], "LG")) {setlgall(linha[2]);}
                else if(Objects.equals(linha[1], "ST")) {setst(linha[2]);}
            }
        }catch (FileNotFoundException e) {
            String[] aux = str.split("\\.");
            log.addFL(aux[0],"!!!!Erro na leitura do ficheiro de configuraçáo do Servidor Secundario!!!!",getDominio());
            e.printStackTrace();
        }
    }

    public void clienteservidor(Query q,Cache ca) {
        Logs log = new Logs();
        while (true) {
            System.out.println("!!!!!!!!!! espera conexão de um cliente !!!!!!!!!!!!!!");
            try {
                DatagramPacket dp = new DatagramPacket(buffer,buffer.length);
                dsocket.receive(dp);
                //log QR
                InetAddress ClienteIp = dp.getAddress();
                int porta = dp.getPort();
                String query = new String(dp.getData(),0,dp.getLength());
                System.out.println("query recebida: " + query);
                log.addQR(ClienteIp.getHostName(),query,getDominio());

                String querydone = q.doQuery(query,ca);
                buffer = querydone.getBytes();
                dp = new DatagramPacket(buffer,buffer.length,ClienteIp,porta);
                System.out.println("resposta enviada: "+ querydone);
                dsocket.send(dp);
                log.addRP(ClienteIp.getHostName(),querydone,getDominio());
            }catch (IOException e) {
                e.printStackTrace();
                break;
            }
        }
    }

    public void transferenciaZona(Cache ca){
        Logs log = new Logs();
        System.out.println("!!!!!!!! iniciei uma transferencia de zona!!!!!!!!!");
        try{
            Socket socket = new Socket(this.sp,12346);                   //no ide
            DataInputStream in = new DataInputStream(socket.getInputStream());         //leitores
            DataOutputStream out = new DataOutputStream(socket.getOutputStream());     //escritor

            //enviar uma mensagem com o proprio dominio
            String qu1 = "domain: "+ getDominio();
            out.writeUTF(qu1);
            System.out.println("perguntei se o sp é do mesmo dominio");

            //envio mensagem a pedir transferencia de zona ao servidor primario
            int count = in.readInt();
            System.out.println("recebi o numero de linhas a ser transferido : "+count);

            //confirmaçao do numero de linhas a receber
            out.writeUTF("ok: " + count);
            System.out.println("confirmei que estou apto a receber as "+count+" linhas");

            //receber todas as linhas
            int i = 0;
            while (i < count) {

                String str = in.readUTF();
                System.out.println("linha "+(i+1)+": "+str);
                ca.ParserPorLinha(str, getDominio());
                i++;

            }
            socket.close(); //fechar a comunicaçao
            //fechar leitores e escritores
            in.close();
            out.close();
        } catch (IOException e) {
            throw new RuntimeException(e);
            log.addEZ(dsocket.getInetAddress().getHostName(),getDominio());
        }
    }

    public static void main(String[] args) throws IOException {
        Query q = new Query();
        Cache ca = new Cache();

        DatagramSocket ds = new DatagramSocket(12347);
        ServerS ss = new ServerS(ds);
        String configfile = "SP.robalo.txt";
        ss.ParserSs(configfile);
        ca.ParserCacheServer(ss.getDb());

        Thread t1 = new Thread(new ServerS.Mover(ss, 0, ca, q)); //thread responsavel pela conexao com os clientes
        Thread t2 = new Thread(new ServerS.Mover(ss, 1, ca, q)); //thread responsavel pela conexao com os servidores

        t1.start();t2.start(); //iniciar as threads
    }
    static class Mover implements Runnable {
        ServerS ss;
        int s;
        Cache ca;
        Query q;
        public Mover(ServerS ss, int s,Cache ca ,Query q) { this.ss=ss; this.s=s;this.ca=ca;this.q=q;}

        public void run() {
            if (s == 0) ss.clienteservidor(q,ca);
            if (s == 1) ss.transferenciaZona(ca);
        }
    }
}
