using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicCollection.Models;
using MusicCollection.ViewModels;

namespace MusicCollection.Controllers {
  public class AlbumController : Controller {
    MusicDbContext db = new MusicDbContext();
    const int resultsPerPage = 56;

    //
    // GET: /Album/

    public ActionResult Index(int page = 1, SortType sortType = SortType.ArtistName, string filter = "") {
      var filteredAlbums = FilterAlbumsByArtist(filter);
      var numberOfAlbums = filteredAlbums.Count();
      var albumResults = SortAlbums(page, sortType, filteredAlbums);

      SetMissingArtworkImage(albumResults);
      var viewModel = new AlbumIndexViewModel {
        Albums = albumResults.ToList(),
        CurrentPage = page,
        CurrentSortType = sortType,
        FriendlySortType = sortType.ToFriendlyName(),
        NumberOfPages = CalculateNumberOfPages(numberOfAlbums),
        AlbumCount = numberOfAlbums,
        CurrentFilter = filter,
      };

      return View(viewModel);
    }

    List<Album> FilterAlbumsByArtist(string artistFilter) {
      if (!string.IsNullOrWhiteSpace(artistFilter)) {
        return db.Albums.Where(a => a.Artist.Contains(artistFilter)).ToList();
      }
      return db.Albums.ToList();
    }

    IEnumerable<Album> SortAlbums(int page, SortType sortType, List<Album> albums) {
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
        default: {
            throw new ArgumentException("Unknown sort type");
          }
      }
      return sortedAlbums.Skip(resultsPerPage * (page - 1)).Take(resultsPerPage);
    }

    void SetMissingArtworkImage(IEnumerable<Album> albumsByArtist) {
      foreach (var album in albumsByArtist) {
        if (string.IsNullOrEmpty(album.ArtworkLocation) || album.ArtworkLocation.Contains("noimage")) {
          album.ArtworkLocation = Url.Encode("Images/album-art-missing.png");
        }
      }
    }

    int CalculateNumberOfPages(int totalAlbums) {
      var pages = totalAlbums / resultsPerPage;
      if (totalAlbums % resultsPerPage != 0) pages++;
      return pages;
    }

    //
    // GET: /Album/Details/5

    public ActionResult Details(long id = 0) {
      Album album = db.Albums.Find(id);
      if (album == null) {
        return HttpNotFound();
      }
      return View(album);
    }

    //
    // GET: /Album/Create

    public ActionResult Create() {
      return View();
    }

    //
    // POST: /Album/Create

    [HttpPost]
    public ActionResult Create(Album album) {
      if (ModelState.IsValid) {
        db.Albums.Add(album);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(album);
    }

    //
    // GET: /Album/Edit/5

    public ActionResult Edit(long id = 0) {
      Album album = db.Albums.Find(id);
      if (album == null) {
        return HttpNotFound();
      }
      return View(album);
    }

    //
    // POST: /Album/Edit/5

    [HttpPost]
    public ActionResult Edit(Album album) {
      if (ModelState.IsValid) {
        db.Entry(album).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(album);
    }

    protected override void Dispose(bool disposing) {
      db.Dispose();
      base.Dispose(disposing);
    }
  }
}