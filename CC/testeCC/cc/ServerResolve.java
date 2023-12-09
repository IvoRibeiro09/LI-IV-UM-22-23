import java.io.*;
import java.net.*;
import java.util.*;

public class ServerResolve {
    private String dominio;
    private String db;
    private HashMap<String,String> ServerIps;
    private String dd;
    private String lg;
    private String lgall;
    private String ST;
    private final DatagramSocket dsocket;
    private byte[] buffer = new byte[5550];

    public ServerResolve(DatagramSocket dsocket){
        this.db = "";
        this.ServerIps = new HashMap<>();
        this.dd = "";
        this.lg = "";
        this.lgall = "";
        this.ST = "";
        this.dsocket = dsocket;
    }

    //seters
    public void setdb(String s) {
        this.db = s;
    }
    public void setDominio(String s){this.dominio = s;}
    public void setServerIps(String key,String value){
        this.ServerIps.put(key,value);
    }
    public void setdd(String s) {
        this.dd = s;
    }
    public void setlg(String s) {
        this.lg = s;
    }
    public void setlgall(String s) {
        this.lgall = s;
    }
    public void setSt(String s){this.ST = s;}

    //geters
    public String getDb() {
        return this.db;
    }
    public String getDominio(){return this.dominio;}

    public void ParserSp(String str) throws IOException {
        Logs log = new Logs();
        try {
            File ficheiro = new File(str);
            Scanner myReader = new Scanner(ficheiro);
            while (myReader.hasNextLine()) {
                String data = myReader.nextLine();
                String[] linha = data.split(" ");
                if (Objects.equals(linha[1], "SP")) {
                    setServerIps(linha[1],linha[2]);
                } else if (Objects.equals(linha[1], "SS")) {
                    setServerIps(linha[1],linha[2]);
                } else if (Objects.equals(linha[1], "DD")) {
                    setdd(linha[2]);
                    setDominio(linha[0]);
                } else if (Objects.equals(linha[0], "all") && Objects.equals(linha[1], "LG")) {
                    setlg(linha[2]);
                } else if (!Objects.equals(linha[0], "all") && Objects.equals(linha[1], "LG")) {
                    setlgall(linha[2]);
                } else if (Objects.equals(linha[1], "ST")) {
                    setSt(linha[2]);
                }
                log.addEV("config",str,getDominio());
            }
        } catch (FileNotFoundException e) {
            String[] aux = str.split("\\.");
            log.addFL(aux[0], "!!!!Erro na leitura do ficheiro de configuraçao do Servidor Primario!!!!",getDominio());
            e.printStackTrace();
        }
    }
    public String SRSPDomain(String query) throws IOException{
        Logs log = new Logs();
        String resposta = "";
        try{
            Socket socket = new Socket(this.ServerIps.get("SP"),12345);
            //Socket socket = new Socket("localhost",12345);  // no ide
            DataInputStream in = new DataInputStream(socket.getInputStream());         //leitores
            DataOutputStream out = new DataOutputStream(socket.getOutputStream());

            //System.out.println("mensagem enviada ao sp do mesmo dominio: "+query);
            System.out.println("SR enviou msg ao SP "+this.ServerIps.get("SP"));


            out.writeUTF(query);
            log.addQE(socket.getInetAddress().getHostName(),query,getDominio());

            resposta = in.readUTF();
            log.addRR(socket.getInetAddress().getHostName(),resposta,getDominio());
            //System.out.println("recebi a seguinte resposta do sp: "+resposta);
            System.out.println("SP "+this.ServerIps.get("SP")+" respondeu ao SR");
            socket.close();
            in.close();
            out.close();
        }catch (IOException e){
            e.printStackTrace();
        }
        return resposta;
    }
    public String SRSP(String query,String ipaux) throws IOException{
        Logs log = new Logs();
        String resposta = "";
        try{
            String IP = "";
            String smaller = "";
            String[] aux = ipaux.split("/");
            IP = aux[0];

            if(Objects.equals(aux[1],"smaller "))smaller = "smaller ";
            Socket socket = new Socket(IP,12345);
            DataInputStream in = new DataInputStream(socket.getInputStream());         //leitores
            DataOutputStream out = new DataOutputStream(socket.getOutputStream());
            //System.out.println("mensagem enviada ao sp "+ IP+": "+smaller+query);
            System.out.println("Mensagem enviada do SR para SP "+IP);
            if(Objects.equals("reverse",aux[1])){
                smaller = "reverse ";
                out.writeUTF(smaller+getip(query));
                log.addQE(socket.getInetAddress().getHostName(),query,getDominio());


            }
            else out.writeUTF(smaller+query);
            log.addQE(socket.getInetAddress().getHostName(),query,getDominio());


            String rsp = in.readUTF();
            //System.out.println("Resposta do sp "+IP+": "+rsp);
            System.out.println("SP "+IP+" respondeu ao SR");
            String[] aux2 = rsp.split("/");
            resposta = aux2[0];
            if(Objects.equals(aux2[1],"smaller")) {
                return SRSP(query,resposta+"/ ");
            }
            log.addRR(socket.getInetAddress().getHostName(),query,getDominio());
            in.close();
            out.close();
            socket.close();
        }catch (IOException e){
            e.printStackTrace();
        }
        return resposta;
    }
    public String SRST(String query){
        Logs log = new Logs();
        String resposta = "";
        try{
            Socket socket = new Socket(this.ST,64321);                   //no ide
            DataInputStream in = new DataInputStream(socket.getInputStream());         //leitores
            DataOutputStream out = new DataOutputStream(socket.getOutputStream());

            System.out.println("SR mandou msg ao ST");
            out.writeUTF(query);
            log.addQE(socket.getInetAddress().getHostName(),query,getDominio());



            resposta = in.readUTF();
            //System.out.println("ST repsondeu ao SR");
            System.out.println("resposta do ST: "+ resposta);
            String[] aux = resposta.split("!");
            if(Objects.equals(aux[0],"iterative")){
                resposta = SRSP(query,aux[1]);
            }
            //System.out.println("recebi a seguinte resposta do st: "+resposta);
            in.close();
            out.close();
            socket.close();
        }catch (IOException e){
            e.printStackTrace();
        }
        return resposta;
    }
    public void clienteServer() {
        Logs log= new Logs();
        while (true) {
            System.out.println("!!!!!!!!!! espera conexão de um cliente !!!!!!!!!!!!!!");
            try {
                buffer = new byte[5550];
                DatagramPacket dp = new DatagramPacket(buffer, buffer.length);
                dsocket.receive(dp);
                //log QR
                InetAddress ClienteIp = dp.getAddress();
                int porta = dp.getPort();
                String query = new String(dp.getData(), 0, dp.getLength());
                System.out.println("query recebida: " + query);
                log.addQR(ClienteIp.getHostName(),query,getDominio());

                String resposta;
                if (isdomain(query) == 0) {
                    //enviar para o sp do dominio
                    resposta = SRSPDomain(query);

                }else {
                    //enviar para o st
                    resposta = SRST(query);
                }
                //enviar resposta
                //por fim enviar a resposta ao cliente
                buffer = new byte[5550];
                buffer = resposta.getBytes();
                dp = new DatagramPacket(buffer,buffer.length,ClienteIp,porta);
                //System.out.println("resposta enviada: "+ resposta);
                System.out.println("Resposta enviada ao cliente pelo SR");
                dsocket.send(dp);
                log.addRP(ClienteIp.getHostName(),resposta,getDominio());

                //log RP
            }catch (IOException e) {
                e.printStackTrace();
                break;
            }
        }
    }
    public String getip(String query){
        String[] aux = query.split(";");
        String[] aux2 = aux[1].split(",");
        return aux2[0];
    }
    public int isdomain(String query){
        String[] aux = query.split(";");
        String[] aux2 = aux[1].split(",");
        if(Objects.equals(aux2[0],this.dominio)) return 0;
        else return 1;
    }
    public static void  main(String[] args) throws IOException {
        DatagramSocket ds = new DatagramSocket(54321);
        ServerResolve sr = new ServerResolve(ds);
        sr.ParserSp(args[0]);
        //sr.ParserSt(sr.st);
        sr.clienteServer();
    }
}
