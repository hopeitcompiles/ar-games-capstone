using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Models
{
    [Serializable]
    public class ApiResponse<T>
    {
        public int code;
        public string message;
        public T data;
        public string codeText;
    }

    [Serializable]
    public class ProfileData
    {
        public int id;
        public string firstname;
        public string lastname;
        public int age;
        public string email;
        public string role;
        public string status;
        public string token;
        public DateTime createDate;
        public string getNames()
        {
            return firstname + " " + lastname;
        }
    }

    [Serializable]
    public class ClassData
    {
        public int id;
        public string className;
        public string grade;
        public string code;
    }

    [Serializable]
    public class GameMetric
    {
        public int id;
        public int gameId;
        public int userId;
        public int classId = 0;
        public double score = 0;
        public double timeElapsed = 0;
        public bool isGameCompleted = false;
        public double percentageOfCompletion = 0;
        public int successCount = 0;
        public int failureCount = 0;
        public string difficulty = "";
        public string comments = "";
    }

}
