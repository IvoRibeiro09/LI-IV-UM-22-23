import java.io.File;
import java.io.FilterOutputStream;
import java.io.IOException;
import java.util.*;


public class Cache {

    private String dfault;
    private String ttl;
    private String soasp;
    private String soaadmin;
    private String soaserial;
    private String soarefresh;
    private String soaretry;
    private String soaexpire;
    private HashMap<String,String> allva;
    private HashMap<String,String> ns;
    private String smaler;
    private HashMap<String,String> mx;
    private HashMap<String,String> Aips;
    private HashMap<String,String> cnames;
    private int lines ;
    private ArrayList<String> AllLines;
    private HashMap<String,Integer> dominios;
    private HashMap<String,String> PTRS;


    //construtor vazio
    public Cache(){
        this.dfault = "";
        this.ttl = "";
        this.soasp = "";
        this.soaadmin = "";
        this.soaserial = "";
        this.soarefresh = "";
        this.soaexpire = "";
        this.allva = new HashMap<>();
        this.ns = new HashMap<>();
        this.smaler = "";
        this.mx = new HashMap<>();
        this.Aips = new HashMap<>();
        this.cnames = new HashMap<>();
        this.lines = 0;
        this.AllLines = new ArrayList<>();
        this.dominios = new HashMap<>();
        this.PTRS = new HashMap<>();
    }
    //seters
    public void setdfault(String s){
        this.dfault = s;
    }
    public void setttl(String s){
        this.ttl = s;
    }
    public void setsoasp(String s){
        this.soasp = s;
    }
    public void setsoaadmin(String s){
        this.soaadmin = s;
    }
    public void setsoaserial(String s){
        this.soaserial = s;
    }
    public void setsoarefresh(String s){
        this.soarefresh = s;
    }
    public void setsoaretry(String s){
        this.soaretry = s;
    }
    public void setsoaexpire(String s){
        this.soasp = s;
    }
    public void setAllva(String key,String value){
        this.allva.put(key,value);
    }
    public void setSmalers(String name){
        this.smaler = name;
    }
    public void setNs(String key,String value){
        this.ns.put(key,value);
    }
    public void setMx(String mx,String value){
        this.mx.put(mx,value);
    }
    public void setAips(String tipo,String ip){
        this.Aips.put(tipo,ip);
    }
    public void setCnames(String tipo,String nome){
        this.cnames.put(tipo,nome);
    }
    public void setAllLines(String s){
        this.AllLines.add(s);
    }
    public void setDominios(String s){this.dominios.put(s,1);}
    public void setPTRS(String key,String value){this.PTRS.put(key,value);}
    public void incLines(){
        this.lines++;
    }

    //geters
    public String getDfault(){
        return this.dfault;
    }
    public HashMap<String,String> getAllva(){
        return allva;
    }
    public String getTtl(){
        return ttl;
    }
    public String getSmaller(){return this.smaler;}
    public HashMap<String,String> getAIps(){
        return Aips;
    }
    public HashMap<String,String> getCnames(){
        return cnames;
    }
    public String getSoaexpire() {
        return this.soaexpire;
    }
    public int getNumeberOfLinesSP() {
        return this.lines;
    }
    public String getCacheLine(int i){
        return AllLines.get(i);
    }
    public HashMap<String,Integer> getDominios(){return this.dominios;}
    public HashMap<String,String> getPTRS(){return this.PTRS;}


    //parser de ficheiros de base de dados de servidores
    public void ParserCacheServer(String strfile) throws IOException {
        Logs log = new Logs();
        System.out.println("----"+strfile);
        int c=0;
        for(int i=0;i<strfile.length();i++){
            if(strfile.charAt(i) == '.')c++;
        }
        String[] aux = strfile.split("\\.");
        String str = "";
        if(c == 3) str = aux[1]+"."+aux[2]+"."; // dominio para confirmar;
        else str = aux[1]+".";

        try {
            File ficheiro = new File(strfile);
            Scanner myReader = new Scanner(ficheiro);

            while (myReader.hasNextLine()) {
                String data = myReader.nextLine();
                ParserPorLinha(data,str);
            }
            //ocorreu sem erros
            log.addEV("bd",str,getDfault());
        } catch (Exception e) {
            //erro
            log.addFL(str,"!!!!Erro no parse do ficheiro de Base de Dados!!!!",getDfault());
        }
    }

    public void ParserPorLinha(String str,String dominio) {
        String[] linha = str.split(" ");
        if (!Objects.equals(linha[0], "#")) {
            if (Objects.equals(linha[0], dominio) || Objects.equals(linha[0],"@")) {
                if (Objects.equals(linha[1], "DEFAULT")) {
                    setdfault(linha[2]);
                    if(Objects.equals(linha[0],dominio))setttl(linha[3]);
                } else if (Objects.equals(linha[1], "SOASP")) {
                    setsoasp(linha[2]);
                    incLines();
                    setAllLines(getDfault()+" "+linha[1]+" "+linha[2]+" "+getTtl());
                } else if (Objects.equals(linha[1], "SOAADMIN")) {
                    setsoaadmin(linha[2]);
                    incLines();
                    setAllLines(getDfault()+" "+linha[1]+" "+linha[2]+" "+getTtl());
                } else if (Objects.equals(linha[1], "SOASERIAL")) {
                    setsoaserial(linha[2]);
                    incLines();
                    setAllLines(getDfault()+" "+linha[1]+" "+linha[2]+" "+getTtl());
                } else if (Objects.equals(linha[1], "SOAREFRESH")) {
                    setsoarefresh(linha[2]);
                    incLines();
                    setAllLines(getDfault()+" "+linha[1]+" "+linha[2]+" "+getTtl());
                } else if (Objects.equals(linha[1], "SOARETRY")) {
                    setsoaretry(linha[2]);
                    incLines();
                    setAllLines(getDfault()+" "+linha[1]+" "+linha[2]+" "+getTtl());
                } else if (Objects.equals(linha[1], "SOAEXPIRE")) {
                    setsoaexpire(linha[2]);
                    incLines();
                    setAllLines(getDfault()+" "+linha[1]+" "+linha[2]+" "+getTtl());
                }else if (Objects.equals(linha[1], "NS")) {
                    String[] splt = linha[2].split("\\.");
                    setNs(splt[0], linha[0]);
                    incLines();
                    setAllva(splt[0], linha[1]);
                    setAllLines(getDfault()+" "+linha[1]+" "+linha[2]+" "+getTtl());
                    setDominios(linha[0]);
                } else if (Objects.equals(linha[1], "MX")) {
                    String[] splt = linha[2].split("\\.");
                    setMx(splt[0], (linha[2] + " " + linha[4]));
                    setAllva(splt[0],linha[1]+"/"+linha[4]);
                    incLines();
                    setAllLines(getDfault()+" "+linha[1]+" "+linha[2]+" "+getTtl()+" "+linha[4]);
                    setDominios(linha[0]);
                }
            }else if (Objects.equals(linha[0], "TTL")) {
                setttl(linha[2]);
            }else if (Objects.equals(linha[0],"Smaller.@")){
                setSmalers(linha[2]);
                incLines();
                setAllLines("Smaller.@ "+linha[1]+" "+linha[2]);
            }  else if (Objects.equals(linha[1], "NS")) {
                String[] splt = linha[2].split("\\.");
                setNs(splt[0], linha[0]);
                incLines();
                setAllva(splt[0], linha[1]);
                setAllLines(getDfault()+" "+linha[1]+" "+linha[2]+" "+getTtl());
                setDominios(linha[0]);
            } else if (Objects.equals(linha[1], "MX")) {
                String[] splt = linha[2].split("\\.");
                setMx(splt[0], (linha[2] + " " + linha[4]));
                setAllva(splt[0],linha[1]+"/"+linha[4]);
                incLines();
                setAllLines(getDfault()+" "+linha[1]+" "+linha[2]+" "+getTtl()+" "+linha[4]);
                setDominios(linha[0]);
            }else if (Objects.equals(linha[1], "A") && Objects.equals(linha[0], "www")) {
                String aux = linha[2] + " " + linha[4];
                setAips(linha[0], aux);
                incLines();
                setAllLines(linha[0]+"."+getDfault()+" "+linha[1]+" "+linha[2]+" "+getTtl()+" "+linha[4]);
            } else if (Objects.equals(linha[1], "A") && !Objects.equals(linha[0], "www")) {
                setAips(linha[0], linha[2]);
                incLines();
                setAllLines(linha[0]+"."+getDfault()+" "+linha[1]+" "+linha[2]+" "+getTtl());
            } else if (Objects.equals(linha[1], "CNAME")) {
                setCnames(linha[2], linha[0]);
                incLines();
                setAllLines(linha[0]+"."+getDfault()+" "+linha[1]+" "+linha[2]+"."+getDfault()+" "+getTtl());
            }else if(Objects.equals(linha[1],"PTR")){
                setPTRS(linha[0],linha[2]);
            }
        }
    }

    public String getSmallerIp() {
        String nome = getSmaller();
        return this.Aips.get(nome);
    }

}
