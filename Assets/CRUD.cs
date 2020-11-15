using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using Mono.Data.SqliteClient;
using System;

public class CRUD : MonoBehaviour{
    public GameObject itemParent, item, form_create, form_edit;
    public IDbConnection _connection = new SqliteConnection("URI=file:MasterSQLite.db");

    DataBase DB = new DataBase();
    void Start(){
        School();
        read();
    }
    
    public void School(){
        StartCoroutine(this.GetSchool());  
    }
    public IEnumerator GetSchool(){
        DB.Escola();
        DB.Turma();
        string ret_school_id,ret_school_name;

        WWWForm form = new WWWForm();
        
        UnityWebRequest www = UnityWebRequest.Get("http://uniescolas.viamaker.com.br/api/obter/escola");
        www.SetRequestHeader("Authorization", "Bearer 8mspL8yN09CgSQ3sgMfwQkfNm2bO64NW2789Wo0EodONKcuKeUtu1taZjG3Wu5XQUi61uxIZiDqxlxuaoZW9LJ5Hj992DNp6H0pk1wA6h4CZdtZkV6fv5xv8mKcFmkQe");

        yield return www.SendWebRequest();

        if (www.isNetworkError) {
            Debug.Log (www.error);
            
        } else {
            var result = JsonConvert.DeserializeObject<Result>(www.downloadHandler.text); 
            JObject jObj = (JObject)JsonConvert.DeserializeObject(result.Escola.Id.ToString());
            int cont = jObj.Count;
            foreach (var register in jObj)
            { 
                ret_school_id = result.Escola.Id.ToString();
                ret_school_name = result.Escola.Nome.ToString();
                Student(ret_school_id);

                DB.InserirEscola(ret_school_id,ret_school_name);
            }


            //Debug.Log(ret_school_name);
        
        }
    }
    public void Student(string ret_school_id)
    {
        StartCoroutine(this.PostStudent(ret_school_id));
    }
    public IEnumerator PostStudent(string  ret_school_id)
    {
        
        string ret_class_id,ret_student_name;

        WWWForm form = new WWWForm();


        form.AddField("TurmaId",ret_school_id );
        UnityWebRequest www = UnityWebRequest.Post("http://uniescolas.viamaker.com.br/api/listar/alunos/turma",form);
        www.SetRequestHeader("Authorization", "Bearer 8mspL8yN09CgSQ3sgMfwQkfNm2bO64NW2789Wo0EodONKcuKeUtu1taZjG3Wu5XQUi61uxIZiDqxlxuaoZW9LJ5Hj992DNp6H0pk1wA6h4CZdtZkV6fv5xv8mKcFmkQe");
        

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);

        }
        else
        {
            var result = JsonConvert.DeserializeObject<Result>(www.downloadHandler.text); 
           /*JObject jObj = (JObject)JsonConvert.DeserializeObject(result.Turmas.Id.ToString()).toArray();
            int cont = jObj.Count;
            for (int i =0;i<cont;i++)
            { 
                ret_class_id = result.Turmas[i].Id.ToString();
                ret_student_name = result.Turmas[i].Nome.ToString();

                //DB.InserirTurma(ret_class_id,ret_student_name,ret_school_id);
            }*/
        }
    }
    public void read(){
        int count = PlayerPrefs.GetInt("count");

        for(int i = 0;i<itemParent.transform.childCount;i++){
            Destroy(itemParent.transform.GetChild(i).gameObject);
        }

        int number = 0;
        for(int i = 0; i<= count; i++){
            number++;
            string id = PlayerPrefs.GetString("id["+i+"]");
            string name = PlayerPrefs.GetString("name["+i+"]");
            string address = PlayerPrefs.GetString("address["+i+"]");

            if(id !=""){
                GameObject tmp_item = Instantiate(item,itemParent.transform);
                tmp_item.name = i.ToString();
                tmp_item.transform.GetChild(0).GetComponent<Text>().text = number.ToString();
                tmp_item.transform.GetChild(1).GetComponent<Text>().text = name;
                tmp_item.transform.GetChild(2).GetComponent<Text>().text = address;
            }
            else{
                number--;
            }
        }
    }
    public void create(){
        int count = PlayerPrefs.GetInt("count");
        count++; 
        PlayerPrefs.SetString("id["+count+"]",count.ToString());
        PlayerPrefs.SetString("name[" + count + "]", form_create.transform.GetChild(1).GetComponent<InputField>().text);
        PlayerPrefs.SetString("address[" + count + "]", form_create.transform.GetChild(2).GetComponent<InputField>().text);
        PlayerPrefs.SetInt("count",count);
        form_create.transform.GetChild(1).GetComponent<InputField>().text = "";
        form_create.transform.GetChild(2).GetComponent<InputField>().text = "";
        read();
    }

    public void delete(GameObject item){
        string id_perf = item.name;
        PlayerPrefs.DeleteKey("id["+id_perf+"]");
        PlayerPrefs.DeleteKey("name["+id_perf+"]");
        PlayerPrefs.DeleteKey("address["+id_perf+"]");
        read();
    }
    string id_edit;


    public void open_form_edit(GameObject obj_edit)
    {
        
        form_edit.SetActive(true);
        id_edit = PlayerPrefs.GetString("id[" + obj_edit.name + "]");
        form_edit.transform.GetChild(1).GetComponent<InputField>().text = PlayerPrefs.GetString("name[" + obj_edit.name + "]");
        form_edit.transform.GetChild(2).GetComponent<InputField>().text = PlayerPrefs.GetString("address[" + obj_edit.name + "]");
       
    }

    public void update()
    {
        PlayerPrefs.SetString("name[" + id_edit + "]", form_edit.transform.GetChild(1).GetComponent<InputField>().text);
        PlayerPrefs.SetString("address[" + id_edit + "]", form_edit.transform.GetChild(1).GetComponent<InputField>().text);
        read();
    }


}
