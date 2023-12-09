import java.io.IOException;
import java.util.*;

public class Query {
    private String ID;
    private String Flags;
    private int nResponse;
    private int  nValues;
    private int nAutho;
    private int nExtravalues;
    private String InfoName;
    private String Type;
    private boolean error;

    //construtor vazio
    public Query(){
        //header Info
        this.ID = "";
        this.Flags="";
        this.nResponse = 0;
        this.nValues = 0;
        this.nAutho = 0;
        this.nExtravalues = 0;
        //Data Info
        this.InfoName = "";
        this.Type ="";
        this.error = false;
    }
    //seters
    public void setID(String s){
        this.ID = s;
    }
    public void setFlags(String s){
        this.Flags = s;
    }
    public void setnResponse(String s){
        this.nResponse = Integer.parseInt(s);
    }
    public void setnValues(String s){
        this.nValues = Integer.parseInt(s);
    }
    public void setnAutho(String s){
        this.nAutho= Integer.parseInt(s);
    }
    public void setnExtravalues(String s){
        this.nExtravalues = Integer.parseInt(s);
    }
    public void setInfoName(String s){
        this.InfoName = s;
    }
    public void setType(String s){
        this.Type = s;
    }
    public void setError(){this.error = true;}

    //funçao que parte a informaçao da query feita pelo cliente
    // e guarda essa informaçao para posterior uso
    private void ParserQuery(String data) throws IOException {
        Logs log = new Logs();
        try{
            String[] parte = data.split(";");
            String[] priParte = parte[0].split(",");
            String[] segParte = parte[1].split(",");
            setID(priParte[0]);
            setFlags(priParte[1]);
            setnResponse(priParte[2]);
            setnValues(priParte[3]);
            setnAutho(priParte[4]);
            setnExtravalues(priParte[5]);
            setInfoName(segParte[0]);
            setType(segParte[1]);
        }catch(Exception e){
            setError();
            log.addFL("mensagem do cliente","mal escrita",getInfoName());
        }
    }
    public String getId(){
        return ID;
    }
    public String getInfoName() {
        return InfoName;
    }
    public String getType() {
        return Type;
    }



    public int dominioQuery(String query,String dominio) throws  IOException{
        ParserQuery(query);
        String nomedominio = getInfoName();
        if(Objects.equals(nomedominio,dominio)&&!this.error){
            return 1;
        } else if(!Objects.equals(nomedominio,dominio)&&!this.error){
            return 0;
        } else return -1;
    }
    public String doQuery(String str,Cache ca) throws IOException {
        ParserQuery(str);
        int nva=0,nres=0,nextra=0,error1=0,error2=0;
        StringBuilder ls = new StringBuilder();
        StringBuilder rv = new StringBuilder();
        StringBuilder av = new StringBuilder();
        StringBuilder ev = new StringBuilder();
        HashMap<String,String> CNAMES = ca.getCnames();


        if(this.error){
            ls.append(getId()).append(",,3,0,0,0;,;");
            return ls.toString();
        }
        this.Flags = this.Flags.replace("Q","R");
        this.Flags = this.Flags+"A";
        if (!Objects.equals(ca.getDfault(),this.InfoName)) error2++;
        else {
            for(String tipo : CNAMES.keySet()){
                String tpaux = ca.getAllva().get(tipo);
                String ttl = "";
                if(!Objects.equals(tpaux,"NS")) {
                    String[] tipoatual = ca.getAllva().get(tipo).split("/");
                    tpaux = tipoatual[0];
                    ttl = tipoatual[1];
                }
                if(Objects.equals(tpaux,this.Type)){
                    error1++;
                    nva++;
                    nextra++;
                    if(Objects.equals(tpaux, "MX"))rv.append(this.InfoName).append(" ").append(tpaux).append(" ").append(tipo).append(".").append(this.InfoName).append(" ").append(ca.getTtl()).append(" ").append(ttl).append(",");
                    else rv.append(this.InfoName).append(" ").append(tpaux).append(" ").append(tipo).append(".").append(this.InfoName).append(" ").append(ca.getTtl()).append(",");
                    ev.append(tipo).append(".").append(this.InfoName).append(" A ").append(ca.getAIps().get(tipo)).append(" ").append(ca.getTtl()).append(",");
                }else {
                    nres++;
                    nextra++;
                    if(Objects.equals(tpaux, "MX")) av.append(this.InfoName).append(" ").append(tpaux).append(" ").append(tipo).append(".").append(this.InfoName).append(" ").append(ca.getTtl()).append(" ").append(ttl).append(",");
                    else av.append(this.InfoName).append(" ").append(tpaux).append(" ").append(tipo).append(".").append(this.InfoName).append(" ").append(ca.getTtl()).append(",");
                    ev.append(tipo).append(".").append(this.InfoName).append(" A ").append(ca.getAIps().get(tipo)).append(" ").append(ca.getTtl()).append(",");
                }
            }
        }
        if(error1 == 0 && error2 == 0) {
            ls.append(getId()).append(",").append(this.Flags).append(",1,0,0,0;").append(this.InfoName).append(",").append(this.Type).append(";");
            return ls.toString();
        }else if(error1 == 0){
            ls.append(getId()).append(",").append(this.Flags).append(",2,0,0,0;").append(this.InfoName).append(",").append(this.Type).append(";");
            return ls.toString();
        }else {
            ls.append(getId()).append(",").append(this.Flags).append(",").append("0").append(",").append(nva)
                    .append(",").append(nres).append(",").append(nextra).append(";")
                    .append(this.InfoName).append(",").append(this.Type).append(";").append(rv)
                    .deleteCharAt(ls.length() - 1).append(";").append(av)
                    .deleteCharAt(ls.length() - 1).append(";").append(ev)
                    .deleteCharAt(ls.length() - 1).append(";");
            return ls.toString();
        }
    }
}
