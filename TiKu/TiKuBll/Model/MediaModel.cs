using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuBll.Model
{
  public class MediaModel
  {
    public System.Int32 fID { get; set; }
    public System.Int32 fCourseId { get; set; }
    public System.String fMediaID { get; set; }
    public System.Int32 fSize { get; set; }
    public System.Int32 fHeight { get; set; }
    public System.Int32 fWidth { get; set; }
    public System.String fUrl { get; set; }
    public System.String fActivityName { get; set; }

  }

  public class MediaListModel
  {
    public List<MediaModel> MediaList { get; set; }
  }
}
