import java.io.IOException;
import java.net.*;
import java.util.Scanner;

public class Cliente {

    private String domain;
    private final DatagramSocket dsocket;
    private final InetAddress ServerIP;
    private int ServerPorta;
    private byte[] buffer = new byte[5550];

    public Cliente(DatagramSocket dsocket,InetAddress ServerIP,int porta){
        this.domain = "bacalhau";
        this.dsocket = dsocket;
        this.ServerIP = ServerIP;
        this.ServerPorta = porta;
    }

    public void clienteservidor(){
        Logs log = new Logs();
        while(true){
            Scanner scanner = new Scanner(System.in);
            System.out.println("digite uma mensagem a enviar para o servidor com IP: " + this.ServerIP);
            try{
                String msg = scanner.nextLine();
                buffer = new byte[5550];
                buffer = msg.getBytes();
                DatagramPacket dp = new DatagramPacket(buffer,buffer.length,this.ServerIP, this.ServerPorta);
                dsocket.send(dp);
               // log.addQE(dsocket.getInetAddress().getHostName(),msg,this.domain);
                System.out.println("query enviada: "+ msg);


                buffer = new byte[5550];
                dp = new DatagramPacket(buffer,buffer.length,this.ServerIP, this.ServerPorta);
                dsocket.receive(dp);

                String resposta = new String(dp.getData(),0, dp.getLength());
                String[] aux = resposta.split("/");
                //log.addRR(dsocket.getInetAddress().getHostName(),aux[0],this.domain);
                System.out.println("resposta recebida: " + aux[0]);

            }catch (IOException e){
                e.printStackTrace();
                break;
            }
        }
    }


    /*
    String qu1 = "3874,Q+R,0,0,0,0;arroz.robalo.,NS;";
    3874,Q,0,0,0,0;arroz.robalo.,MX;
    3874,Q,0,0,0,0;robalo.,MX;
     */
    public static void main(String[] args) throws UnknownHostException, SocketException {
        DatagramSocket dsocket = new DatagramSocket();
        InetAddress ServerIP = InetAddress.getByName(args[0]);
        int porta = 54321;
        Cliente cliente = new Cliente(dsocket,ServerIP,porta);
        cliente.clienteservidor();
    }
    //3874,Q+R,0,0,0,0;10.0.16.11,PTR;

}
