using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicCollection.Models {
  [MetadataType(typeof(AlbumMetaData))]
  public partial class Album { }
  public class AlbumMetaData {
    [Required]
    [DisplayName("Album Name")]
    public object Name { get; set; }
    [Required]
    public object Artist { get; set; }
    [DisplayName("Album Artist")]
    public object AlbumArtist { get; set; }
    [DisplayName("Play Count")]
    public object PlayCount { get; set; }
    [DisplayName("Date Added")]
    [DataType(DataType.Date)]
    public object DateAdded { get; set; }
    [DataType(DataType.Date)]
    [DisplayName("Last Played")]
    public object LastPlayed { get; set; }
    [DisplayName("Release Year")]
    public object Year { get; set; }
  }
}