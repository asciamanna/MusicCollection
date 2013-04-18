using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace MusicCollection.Models {
  public class Album {
    public long AlbumID { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    [DisplayName("Album Artist")]
    public string AlbumArtist { get; set; }
    public string Genre { get; set; }
    [DisplayName("Release Year")]
    public int? Year { get; set; }
    [DisplayName("Play Count")]
    public int PlayCount { get; set; }
    [DataType(DataType.Date)]
    [DisplayName("Date Added")]
    public DateTime DateAdded { get; set; }
    [DataType(DataType.Date)]
    [DisplayName("Last Played")]
    public DateTime? LastPlayed { get; set; }
    [DisplayName("Artwork Location")]
    public string ArtworkLocation { get; set; }
  }

  public class MusicDbContext : DbContext {
    public DbSet<Album> Albums { get; set; }
    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }

  }
}