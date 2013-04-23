using MusicCollection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicCollection.ViewModels {
  public class AlbumIndexViewModel {
    public List<Album> Albums { get; set; }
    public int AlbumCount { get; set; }
    public int CurrentPage { get; set; }
    public int NumberOfPages { get; set; }
    public SortType CurrentSortType { get; set; }
    public string FriendlySortType { get; set; }
  }
}