using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models
{
    public class ApiResponse<T>
    {
        public int code { get; set; }
        public string message { get; set; }
        public T data { get; set; }
        public string codeText { get; set; }
    }
    public class ProfileData 
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int age { get; set; }
        public string email { get; set; }
        public Role role { get; set; }
        public string status { get; set; }
        public string token { get; set; }
        public DateTime createDate { get; set; }
        public string getNames()
        {
            return firstname + " " + lastname;
        }
    }
    public class ClassData
    {
        public int id { get; set; }
        public string className { get; set; }
        public string grade { get; set; }
        public string code { get; set; }
    }
}
