using MusicCollection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicCollection.Repositories {
  public interface IAlbumRepository {
    List<Album> FindAlbumsByArtist(string artistName, SortType sortType);
  }

  public class AlbumRepository : IAlbumRepository {
    public List<Album> FindAlbumsByArtist(string artistName, SortType sortType) {
      var foundAlbums = new List<Album>();
      using (var db = new MusicDbContext()) {
        var albums = db.Albums.Include("Tracks");
        foundAlbums = String.IsNullOrWhiteSpace(artistName) ? albums.ToList() : albums.Where(a => a.Artist.Contains(artistName)).ToList();
        foundAlbums.ForEach(a => a.Tracks = a.Tracks.OrderBy(t => t.TrackNumber).ToList());
      }
      return SortAlbums(sortType, foundAlbums);
    }

    List<Album> SortAlbums(SortType sortType, List<Album> albums) {
      IOrderedEnumerable<Album> sortedAlbums;
      switch (sortType) {
        case SortType.ArtistName: {
            sortedAlbums = albums.OrderBy(a => a.Artist).ThenBy(a => a.Year);
            break;
          }
        case SortType.ReleaseYear: {
            sortedAlbums = albums.OrderBy(a => a.Year);
            break;
          }
        case SortType.LastAdded: {
            sortedAlbums = albums.OrderByDescending(a => a.DateAdded);
            break;
          }
        case SortType.LastPlayed: {
            sortedAlbums = albums.OrderByDescending(a => a.LastPlayed);
            break;
          }
        case SortType.MostPlayed: {
            sortedAlbums = albums.OrderByDescending(a => a.PlayCount);
            break;
          }
        default: {
            throw new ArgumentException("Unknown sort type");
          }
      }
      return sortedAlbums.ToList();
    }
  }
}