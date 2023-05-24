using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IHLA_Template.Models
{
    public class FamilyList
    {
        public class Guardian
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Relation { get; set; }
        }
        public int ChildID { get; set; }
        public string ChildName { get; set; }
        public int ChildAge { get; set; }
        public string ChildClassroom { get; set; }
        public List<Guardian> Parents { get; set; }
        public Guardian PickUpPerson { get; set; }

        public static List<FamilyList> GetFamilyLists()
        {
            List<FamilyList> families = new List<FamilyList>();
            try
            {
                Database db = new Database();
                SqlConnection cn = null;
                if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
                SqlCommand cm = new SqlCommand("SELECT TChildren.intChildID, TChildren.strFirstName, TChildren.strLastName, TChildren.dtmDOB FROM TChildren", cn);

                SqlDataReader result = cm.ExecuteReader();
                if(result.HasRows)
                while (result.Read())
                {
                    FamilyList newFamily = new FamilyList()
                    {
                        ChildID = result.GetInt32(0),
                        ChildName = result.GetString(1) + " " + result.GetString(2),
                        ChildAge = DateTime.Now.Subtract(result.GetDateTime(3)).Days/365,
                        ChildClassroom = GetTeacher(result.GetInt32(0)),
                        Parents = GetParents(result.GetInt32(0)),
                        PickUpPerson = GetParents(result.GetInt32(0)).First()
                    };
                    families.Add(newFamily);
                }
                db.CloseDBConnection(ref cn);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return families;
        }
        private static List<Guardian> GetParents(int childID)
        {
            List<Guardian> parents = new List<Guardian>();
            try
            {
                Database db = new Database();
                SqlConnection cn = null;
                if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
                SqlCommand cm = new SqlCommand("SELECT TParents.intParentID, TParents.strFirstName, TParents.strLastName FROM TParents INNER JOIN TParentChild on TParents.intParentID = TParentChild.intParentID WHERE TParentChild.intChildID=" + childID, cn);

                SqlDataReader result = cm.ExecuteReader();
                
                while (result.HasRows && result.Read())
                {
                    Guardian parent = new Guardian()
                    {
                        ID = result.GetInt32(0),
                        Name = result.GetString(1) + " " + result.GetString(2),
                        Relation = ""//result.GetString(3)
                    };
                    parent.ID = parent.ID;
                    parents.Add(parent);
                }
                db.CloseDBConnection(ref cn);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            if (parents.Count < 1)
            {
                Guardian guardian = new Guardian()
                {
                    ID = -1,
                    Name = "None",
                    Relation = "None"
                };
                parents.Add(guardian);
            }
            return parents;
        }

        private static String GetTeacher(int childID)
        {
            String Teacher = "";
            try
            {
                Database db = new Database();
                SqlConnection cn = null;
                if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
                SqlDataReader result = new SqlCommand("SELECT TEmployees.strFirstName, TEmployees.strLastName FROM TEmployees INNER JOIN TEmployeeChildAssignment ON TEmployees.intEmployeeID = TEmployeeChildAssignment.intEmployeeID WHERE TEmployeeChildAssignment.intChildID=" + childID, cn).ExecuteReader();
                while (result.HasRows && result.Read())
                    Teacher += result.GetString(0) + " " + result.GetString(1)+ ". ";
                db.CloseDBConnection(ref cn);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            if (Teacher == "")Teacher = "Not Assigned";
            return Teacher;
        }
    }
}