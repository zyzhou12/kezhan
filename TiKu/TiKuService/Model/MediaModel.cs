using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuService.Model
{
  public class MediaResponseModel
  {
    public string RequestId { get; set; }
    public MediaList MediaList { get; set; }
  }

  public class MediaList
  {
    public List<MediaModel> Media { get; set; }
  }

  public class MediaModel
  {
    public string CoverURL { get; set; }
    public string MediaId { get; set; }
    public int Heigth { get; set; }
    public int Width { get; set; }
    public int Size { get; set; }
    public PlayList PlayList { get; set; }
  }

  public class PlayList
  {
    public List<Play> Play { get; set; }
  }

  public class Play
  {
    public string Format { get; set; }
    public FileModel File { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public int Size { get; set; }
    public string ActivityName { get; set; }
  }

  public class FileModel
  {
    public string State { get; set; }
    public string URL { get; set; }
  }
}