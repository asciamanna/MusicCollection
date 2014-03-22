using MusicCollection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;

namespace MusicCollection.Repositories {
  public interface IAlbumRepository {
    List<Album> FindAlbumsByArtist(string artistName, SortType sortType, int page);
    int AlbumCount();
    int PagesOfAlbums();
  }

  public class AlbumRepository : IAlbumRepository {
    const int resultsPerPage = 52;

    int? numberOfAlbums;

    int NumberOfAlbums {
      get {
        if (!numberOfAlbums.HasValue) {
          using (var context = new MusicDbContext()) {
            numberOfAlbums = context.Albums.Count();
          }
        }
        return numberOfAlbums.Value;
      }
    }

    public List<Album> FindAlbumsByArtist(string artistName, SortType sortType, int page) {
      using (var db = new MusicDbContext()) {
        var query = db.Albums.Include("Tracks");
        var queryByArtist = String.IsNullOrWhiteSpace(artistName) ? query.AsQueryable<Album>() : query.Where(a => a.Artist.Contains(artistName));
        numberOfAlbums = queryByArtist.Count();
        var pagedQuery = PageAlbums(SortAlbums(sortType, queryByArtist), page);
      
        return pagedQuery.ToList();
      }
    }

    IOrderedQueryable<Album> SortAlbums(SortType sortType, IQueryable<Album> albumsQuery) {
      IOrderedQueryable<Album> sortedAlbumsQuery;
      switch (sortType) {
        case SortType.ArtistName: {
            sortedAlbumsQuery = albumsQuery.OrderBy(a => a.Artist).ThenBy(a => a.Year);
            break;
          }
        case SortType.ReleaseYear: {
            sortedAlbumsQuery = albumsQuery.OrderBy(a => a.Year);
            break;
          }
        case SortType.LastAdded: {
            sortedAlbumsQuery = albumsQuery.OrderByDescending(a => a.DateAdded);
            break;
          }
        case SortType.LastPlayed: {
            sortedAlbumsQuery = albumsQuery.OrderByDescending(a => a.LastPlayed);
            break;
          }
        case SortType.MostPlayed: {
            sortedAlbumsQuery = albumsQuery.OrderByDescending(a => a.PlayCount);
            break;
          }
        default: {
            throw new ArgumentException("Unknown sort type");
          }
      }
      return sortedAlbumsQuery;
    }

    IQueryable<Album> PageAlbums(IOrderedQueryable<Album> query, int page) {
      return query.Skip(resultsPerPage * (page - 1)).Take(resultsPerPage);
    }

    public int AlbumCount() {
      return NumberOfAlbums;
    }

    public int PagesOfAlbums() {
      var pages = NumberOfAlbums / resultsPerPage;
      if (NumberOfAlbums % resultsPerPage != 0) pages++;
      return pages;
    }
  }
}