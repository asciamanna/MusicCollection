using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCollection.Models {
  public class AlbumTrack {
    public long AlbumTrackId { get; set; }
    public string Name { get; set; }
    public string PlayingTime { get; set; }
    public int TrackNumber { get; set; }
    public long AlbumID { get; set; }
  }
}
