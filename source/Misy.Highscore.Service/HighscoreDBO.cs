namespace Misy.Highscore.Service;

public record HighscoreDBO(DateTime? created, string appname, string user, long score);