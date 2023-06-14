 using System.Collections;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;

     
namespace NetworkTools
{
    public enum NetworkResult
    {
        Initial,
        Waiting,
        Success,
        EmptyUriError,
        ConnectionError,
        DataProcessingError,
        ProtocolError
    }

    public class HttpManager
    {
        // Check the network result
        public NetworkResult Result => _result;
        private NetworkResult _result = NetworkResult.Initial;

        // Data Part
        private SendMessage _message;
        public RecvMessage Data => _data;
        private RecvMessage _data;
        public JsonData JsonData => _jsonData;
        public byte[] RawData => _rawData;
        private JsonData _jsonData;
        private byte[] _rawData;
        

        public HttpManager(RecvMessage rmsg, SendMessage message = null)
        {
            _message = message;
            _data = rmsg;
        }

        /// <summary>
        /// Used for preparation of GET uri
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="subCatalogue"></param>
        /// <returns></returns>
        public string GetParser(string ip, string port, string[] subCatalogue = null)
        {
            if (string.IsNullOrEmpty(ip) && string.IsNullOrEmpty(port))
            {
                return string.Empty;
            }

            string uri = UriParser(ip, port, subCatalogue);
            if (_message != null)
            {
                uri += "?";
                uri += _message.ToJsonString();
                Debug.Log($"To ip:{ip}, port:{port} message sent {_message.ToJsonString()}");
            }
            return uri;
        }


        public (string, WWWForm) PostParser(string ip, string port, string[] subCatalogue = null)
        {
            if (string.IsNullOrEmpty(ip) && string.IsNullOrEmpty(port))
            {
                return (string.Empty, null);
            }

            string uri = UriParser(ip, port, subCatalogue);
            WWWForm form = null;
            if (_message != null)
            {
                form = _message.ToWWWForm();
            }
            return (uri, form);
        }


        private static string UriParser(string ip, string port, string[] subCatalogue = null)
        {
            if (string.IsNullOrEmpty(ip) && string.IsNullOrEmpty(port))
            {
                return string.Empty;
            }

            string uri = $"http://{ip}:{port}/";
            if (subCatalogue != null)
            {
                foreach (string catalogue in subCatalogue)
                {
                    uri += $"{catalogue}/";
                }
            }

            return uri;
        }

        public IEnumerator Get(string uri)
        {
            _result = NetworkResult.Waiting;
            if (string.IsNullOrEmpty(uri))
            {
                _result = NetworkResult.EmptyUriError;
                yield break;
            }

            using UnityWebRequest webRequest = UnityWebRequest.Get(uri);   
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Get web request
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    _result = NetworkResult.ConnectionError;
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    _result = NetworkResult.DataProcessingError;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    _result = NetworkResult.ProtocolError;
                    break;
                case UnityWebRequest.Result.Success:
                    _jsonData = JsonMapper.ToObject(webRequest.downloadHandler.text);
                    _data.SetJsonData(_jsonData);
                    _rawData = webRequest.downloadHandler.data;
                    _result = NetworkResult.Success;
                    break;
            }
        }

        public IEnumerator Post(string uri, WWWForm form)
        {
            _result = NetworkResult.Waiting;
            if (string.IsNullOrEmpty(uri))
            {
                _result = NetworkResult.EmptyUriError;
                yield break;
            }

            using UnityWebRequest webRequest = UnityWebRequest.Post(uri, form);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Get web request
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    _result = NetworkResult.ConnectionError;
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    _result = NetworkResult.DataProcessingError;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    _result = NetworkResult.ProtocolError;
                    break;
                case UnityWebRequest.Result.Success:
                    _jsonData = JsonMapper.ToObject(webRequest.downloadHandler.text);
                    _data.SetJsonData(_jsonData);
                    _rawData = webRequest.downloadHandler.data;
                    _result = NetworkResult.Success;
                    break;
            }
        }


        public void Clear()
        {
            _result = NetworkResult.Initial;
            _data = null;
            _jsonData = null;
            _rawData = null;
        }
    }
}