using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Data;
using Mono.Data.SqliteClient;
using System;

public class DataBase
{
    public IDbConnection _connection = new SqliteConnection("URI=file:MasterSQLite.db");

    // Start is called before the first frame update
    
    public void Escola()//Criando Tabela Escola
    {
        
        IDbCommand _command = _connection.CreateCommand();

        _connection.Open();
        string sql = "CREATE TABLE IF NOT EXISTS school(id INT, name VARCHAR(30))";
        _command.CommandText = sql;
        _command.ExecuteNonQuery();
        _connection.Close();

    }
    public void Turma()//Criando Tabela Turma
    {
        
        IDbCommand _command = _connection.CreateCommand();

        _connection.Open();
        string sql = "CREATE TABLE IF NOT EXISTS school_class(id INT, id_school INT,class VARCHAR(30), name VARCHAR(30))";
        _command.CommandText = sql;
        _command.ExecuteNonQuery();
        _connection.Close();

    }
    
    public void InserirEscola(string ret_school_id,string ret_school_name)//Realiza insert na tabeka school
    {
        
        IDbCommand _command = _connection.CreateCommand();
        
        _connection.Open();
        string sql = "INSERT INTO school(id , name ) VALUES('"+ Convert.ToInt32(ret_school_id)+"','"+ret_school_name+"')";
        _command.CommandText = sql;
        _command.ExecuteNonQuery();
        _connection.Close();

    }
    public void InserirTurma(string ret_class_id,string ret_student_name,string ret_school_id)//Realiza insert na tabela school_class
    {

        string StudentClass = (ret_student_name.Split("-"[0])).ToString();
        string StudentName = (ret_student_name.Split("-"[1])).ToString();  
        IDbCommand _command = _connection.CreateCommand();

        _connection.Open();
        string sql = "INSERT INTO school_class(id , id_school ,classe , name ) VALUES('"+ Convert.ToInt32(ret_class_id)+"','"+ Convert.ToInt32(ret_school_id)+"','"+StudentClass+"','"+StudentName+"')";
        _command.CommandText = sql;
        _command.ExecuteNonQuery();
        _connection.Close();

    }
    public void RecuperarEscola()
    {
        IDbCommand _command = _connection.CreateCommand();

        _connection.Open();
        string sqlQuery = "SELECT id,name FROM school ";
        _command.CommandText = sqlQuery;
        IDataReader reader = _command.ExecuteReader();
        while (reader.Read()){
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
             

        }
        
        _connection.Close();
    } 

    public void RecuperarClasse(int escola)
    {
        IDbCommand _command = _connection.CreateCommand();

        _connection.Open();
        string sqlQuery = "SELECT b.id,a.id_school,a.classe,a.name FROM school_class a join school b on a.id_school = b.id where b.id = '"+escola+"'";
        _command.CommandText = sqlQuery;
        IDataReader reader = _command.ExecuteReader();
        while (reader.Read()){
            int id = reader.GetInt32(0);
            int id_school = reader.GetInt32(1);
            string classe = reader.GetString(2);
            string name = reader.GetString(3);
           

        }
        
        _connection.Close();
    }     
    

}



