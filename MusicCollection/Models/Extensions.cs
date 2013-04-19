using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicCollection.Models {
  public static class Extensions {
    public static string ToFriendlyName(this SortType sortType) {
      string result = String.Empty;
      foreach (var c in sortType.ToString()) {
        if (char.IsUpper(c)) {
          result += " ";
        }
        result += c.ToString();
      }
      return result.Trim();
    }
  }
}