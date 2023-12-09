import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.*;

public class ServerTopo {
        private String DB;
        private final ServerSocket ssocket;
        private byte[] buffer = new byte[550];

        public ServerTopo(ServerSocket ss) {
            this.DB = "";
            this.ssocket = ss;
        }

        public void setDB(String DB) {
            this.DB = DB;
        }
        public String getDb(){
            return this.DB;
        }

    public void ParserST(String str) throws IOException {
        Logs log = new Logs();
        try {
            File ficheiro = new File(str);
            Scanner myReader = new Scanner(ficheiro);
            while (myReader.hasNextLine()) {
                String data = myReader.nextLine();
                String[] linha = data.split(" ");
                if (Objects.equals(linha[1], "DB")) {
                    setDB(linha[2]);
                }
            }
            log.addEV("config",str,"root");
        } catch (FileNotFoundException e) {
            String[] aux = str.split("\\.");
            log.addFL( "127.0.0.1", "!!!!Erro na leitura do ficheiro de configuraçao do Servidor de Topo!!!!","root");
            e.printStackTrace();
        }
    }
    public void STservidor(Cache ca){
            Logs log = new Logs();
        while(true){
            System.out.println("!!!!!!! ST espera conexão!!!!!!!!!!");
            try {
                Socket socket = ssocket.accept();
                DataInputStream in = new DataInputStream(socket.getInputStream());
                DataOutputStream out = new DataOutputStream(socket.getOutputStream());

                String query = in.readUTF();
                log.addQR(socket.getInetAddress().getHostName(),query,"root");
                //System.out.println("query recebida: "+ query);
                System.out.println("St recebeu uma query"+query);
                if(Objects.equals(gettipo(query),"PTR")){
                    if(iterative(query) == 1)out.writeUTF("iterative!"+ca.getAIps().get("sp.reverse.")+"/reverse");
                    else {
                        Socket s2 = new Socket(ca.getAIps().get("sp.reverse."), 12345);
                        DataInputStream in2 = new DataInputStream(s2.getInputStream());
                        DataOutputStream out2 = new DataOutputStream(s2.getOutputStream());
                        System.out.println("mandei msg ao reverse");
                        out2.writeUTF("reverse " + getip(query));
                        log.addQE(s2.getInetAddress().getHostName(),query,"root");

                        String str = in2.readUTF();
                        System.out.println("recebi msg ao reverse: " + str);
                        out.writeUTF(str);
                        log.addRR(s2.getInetAddress().getHostName(),str,"root");
                    }
                }else if(!ca.getDominios().containsKey(getdomain(query))) {
                    out.writeUTF(",,3,0,0,0;"+getdomain(query)+",;");
                }else {
                    //String resposta;
                    if (iterative(query) == 1) {
                        System.out.println("query é iterativa");
                        String ip = ca.getAIps().get("sp." + getdomain(query));
                        if (isSub(query) == 2) out.writeUTF("iterative!" + ip + "/smaller ");
                        else out.writeUTF("iterative!" + ip + "/ ");
                        System.out.println("St respondeu " + ip);
                    } else {
                        System.out.println("query nao é iterativa");
                        String ip = "";
                        if (isSub(query) == 2) {
                            Socket s2 = new Socket(ca.getAIps().get("sp." + getdomain(query)), 12345); //ter um getip invez do localhost e uma porta no ficheiro
                            DataInputStream in2 = new DataInputStream(s2.getInputStream());
                            DataOutputStream out2 = new DataOutputStream(s2.getOutputStream());

                            out2.writeUTF("smaller ");
                            String a = in2.readUTF(); //ip do sp do subdominio
                            String[] b = a.split("/");
                            ip = b[0];
                        } else {
                            ip = ca.getAIps().get("sp." + getdomain(query));
                        }
                        //enviar a query ao sp do dominio certo
                        Socket s3 = new Socket(ip, 12345);
                        DataInputStream in3 = new DataInputStream(s3.getInputStream());
                        DataOutputStream out3 = new DataOutputStream(s3.getOutputStream());
                        out3.writeUTF(query);
                        log.addQR(s3.getInetAddress().getHostName(),query,"root");
                        String resposta = in3.readUTF();
                        log.addRR(s3.getInetAddress().getHostName(),resposta,"root");
                        System.out.println("St respondeu");
                        out.writeUTF(resposta);//enviar a resposta
                        log.addRP(socket.getInetAddress().getHostName(),resposta,"root");
                    }
                }
            }catch (IOException e){
                e.printStackTrace();
            }
        }
    }
    public int isSub(String query){
            String[] aux = query.split(";");
            String[] aux2 = aux[1].split(",");
            int c = 0;
            for(int i =0;i < aux2[0].length();i++){
                if(aux2[0].charAt(i)=='.'){
                    c++;
                }
            }
            return c;
    }
    public String getdomain(String query){
            String[] aux = query.split(";");
            String[] aux2 = aux[1].split(",");
            String[] aux3 = aux2[0].split("\\.",2);
            if(isSub(query) == 2)return aux3[1];
            else return aux3[0]+".";

    }
    public int iterative(String query){
        String[] aux = query.split(";");
        String[] aux2 = aux[0].split(",");
        for(int i=0;i < aux2[1].length();i++){
            if(aux2[1].charAt(i)=='R'){
                return 0;
            }
        }
        return 1;
    }
    public String gettipo(String query){
        String[] aux = query.split(";");
        String[] aux2 = aux[1].split(",");
        return aux2[1];
    }
    public String getip(String query){
        String[] aux = query.split(";");
        String[] aux2 = aux[1].split(",");
        return aux2[0];
    }
    public static int getPorta(String IP){
            String[] aux = IP.split("\\.");
            return Integer.parseInt(aux[2])*100 + Integer.parseInt(aux[3]);
    }
    public static void main (String[] args) throws IOException{
            Cache ca = new Cache();
            ServerSocket serverSocket = new ServerSocket(64321);
            ServerTopo St = new ServerTopo(serverSocket);
            String config = "ST.txt";
            St.ParserST(config);
            ca.ParserCacheServer(St.getDb());
            St.STservidor(ca);
    }
}
