// using DefaultNamespace;
// using UnityEngine;
//
// namespace NetworkTools
// {
//     public class SMapMessage : SendMessage
//     {
//         public int map;
//         public int index;
//         public string user;
//
//
//         public SMapMessage(int map, int index)
//         {
//             this.map = map;
//             this.index = index;
//             user = GameManager.Instance.user;
//         }
//         
//         public override WWWForm ToWWWForm()
//         {
//             WWWForm form = new WWWForm();
//             form.AddField("map", map);
//             form.AddField("index", index);
//             return form;
//         }
//     }
// }