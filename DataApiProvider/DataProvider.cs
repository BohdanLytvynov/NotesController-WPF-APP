using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using LogModels;
using MethodCallLibrary;
using Models.ServerModel;

namespace DataApiProvider
{
    public class DataProvider
    {
        private string m_urltohost;//url host

        public string URLHost { get=> m_urltohost; set=> m_urltohost = value; }

        private HttpClient m_client;//Http client
        

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="url"></param>
        public DataProvider(string url)
        {
            m_client = new HttpClient();

            m_urltohost = url;
        }

        /// <summary>
        /// Get Notes From Server with operation logging
        /// </summary>
        /// <param name="logCol"></param>
        /// <returns></returns>
        public IEnumerable<Note> GetNotesFromServer(List<LogBase> logCol
            )
        {
            bool result = false;

            string json = String.Empty;
            
            string url = m_urltohost + "api/Notes";

            json = MethodCaller.CallFuncMethodViaEnvelopeWithLoging
                <Func<string>, LogBase, List<LogBase>,
                Func<Exception, object[], LogBase>, string>
                (GetRequest, out result, logCol, CreateLog, new object[3] { "Get", "200", "404" }
                );

            if(result)
                return JsonConvert.DeserializeObject<IEnumerable<Note>>(json);

            return null;
        }
        /// <summary>
        /// Method that is used to create a propriate log
        /// </summary>
        /// <param name="e"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private LogBase CreateLog(Exception e, object [] info)
        {
            HTTPConnectionLog log = null;

            if (e==null)
            {
                log = new HTTPConnectionLog("", info[1] as string, info[0] as string);
            }
            else
            {
                log = new HTTPConnectionLog(e.Message + e.InnerException.Message, info[2] as string, info[0] as string);
            }

            return log;
        }
        /// <summary>
        /// Get request to api that will get notes collection
        /// </summary>
        /// <returns></returns>
        private string GetRequest()
        {
            return m_client.GetStringAsync(URLHost + "api/Notes").Result;            
        }
        
        /// <summary>
        /// Sends post Authorization request to
        /// server and after performs a get request to server to get
        /// the Authorization result with logging
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logCol"></param>
        /// <returns></returns>
        public List<bool> PostUserAndGetAuthStatusEnvelope(User user, List<LogBase> logCol)
        {
            bool _result = false;

            return MethodCaller.CallFuncMethodViaEnvelopeWithLoging
                <Func<User, List<bool>>, LogBase, List<LogBase>, 
                Func<Exception, object [] , LogBase>, List<bool>>
                (PostUserAndGetUserStatus, out _result, logCol, CreateLog,
                new object[3] { "Post/Get", "200", "404" }, user );
        }

        /// <summary>
        /// Post Auth request and get User status (Admin or user)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private List<bool> PostUserAndGetUserStatus(User user)
        {
            HttpResponseMessage res = m_client.PostAsync(
                requestUri: m_urltohost + "api/Sec",
                content: new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8,
                mediaType: "application/json")
                ).Result;

            if (res.StatusCode.Equals(HttpStatusCode.OK))
            {
                string json = m_client.GetStringAsync(m_urltohost + "api/Sec/GetAuthResponse/" + "{" + user.Login + "}").Result;

                return JsonConvert.DeserializeObject<List<bool>>(json);
            }

            return null;

        }
        /// <summary>
        /// Request for adding notes to db
        /// </summary>
        /// <param name="notesForAdding"></param>
        /// <returns></returns>
        private HttpResponseMessage AddRequest(List<Notes> notesForAdding)
        {
           return m_client.PostAsync(URLHost+"api/Notes/AddNotes", 
                new StringContent(JsonConvert.SerializeObject(notesForAdding),
                Encoding.UTF8, mediaType:"application/json"
                )).Result;
        }

        /// <summary>
        /// Request for editing notes to db
        /// </summary>
        /// <param name="notesForUpdate"></param>
        /// <returns></returns>
        private HttpResponseMessage EditRequest(List<Notes> notesForUpdate)
        {
            return m_client.PostAsync(URLHost + "api/Notes/Edit",
                new StringContent(JsonConvert.SerializeObject(notesForUpdate),
                Encoding.UTF8, mediaType: "application/json"
                )).Result;
        }

        /// <summary>
        /// Request for removing notes
        /// </summary>
        /// <param name="notesForRemove"></param>
        /// <returns></returns>
        private HttpResponseMessage RemoveRequest(List<Guid> notesForRemove)
        {
            return m_client.PostAsync(URLHost + "api/Notes/RemoveNotes",
                new StringContent(JsonConvert.SerializeObject(notesForRemove),
                Encoding.UTF8, mediaType: "application/json"
                )).Result;            
        }
        /// <summary>
        /// Add Notes Request with logging
        /// </summary>
        /// <param name="LogCollection"></param>
        /// <param name="notesForAdding"></param>
        /// <returns></returns>
        public bool AddNotesRequest(List<LogBase> LogCollection, List<Notes> notesForAdding)
        {
            bool result = false;

            MethodCaller.CallFuncMethodViaEnvelopeWithLoging
                <Func<List<Notes>, HttpResponseMessage>, LogBase, 
                List<LogBase>, Func<Exception, object[], LogBase>,
                HttpResponseMessage
                >
                (
                    AddRequest, out result, LogCollection, CreateLog,
                    new object[3] { "Add_Notes_Request", "200", "404" }, notesForAdding
                );

            return result;
        }

        /// <summary>
        /// Edit Request with logging
        /// </summary>
        /// <param name="LogCollection"></param>
        /// <param name="notesForEditing"></param>
        /// <returns></returns>
        public bool EditNotesRequest(List<LogBase> LogCollection, List<Notes> notesForEditing)
        {
            bool result = false;

            MethodCaller.CallFuncMethodViaEnvelopeWithLoging
                <Func<List<Notes>, HttpResponseMessage>, LogBase, List<LogBase>
                , Func<Exception, object[], LogBase>, HttpResponseMessage>
                   
                (EditRequest, out result, LogCollection, CreateLog, new object[3] { "Edit_Request", "200", "404"},
                notesForEditing);

            return result;
        }

        /// <summary>
        /// Remove request with logging
        /// </summary>
        /// <param name="LogCollection"></param>
        /// <param name="notesForRemove"></param>
        /// <returns></returns>
        public bool RemoveRequest(List<LogBase> LogCollection, List<Guid> notesForRemove)
        {
            bool result = false;

            MethodCaller.CallFuncMethodViaEnvelopeWithLoging
                <Func<List<Guid> ,HttpResponseMessage>, LogBase, List<LogBase>, 
                Func<Exception, object [], LogBase>, HttpResponseMessage>
                (RemoveRequest, out result, LogCollection, CreateLog,
                new object[3] { "Remove_Request", "200", "404" },
                notesForRemove);

            return result;
        }
    }
}
