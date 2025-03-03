Ablauf Differenzübersetzung gegen Steamordner:
 - STEAM_DIFF:		->	Erstellt Datei _SteamKeysToCreate.yml und _SteamKeysToDelete.yml im Verzeichnis analyse
					Diese Datei auf korrekte strings prüfen ("" am Dateiende, ...)
 - STEAM_REMOVE:	->	Entfernt alle im Steam-Ordner nicht mehr vorhandenen Schlüeesl _SteamKeysToDelete.yml.sub
 - STEAM_SUB:		->	Erstellt Datei _SteamKeysToCreate.yml.sub (Datei mit Substitutionen)
					->	_SteamKeysToCreate.yml.CC -> ColoCode-Datei (aktuell leer da nur die ColorCode-EndeTags übersetzt werden)
					->	_SteamKeysToCreate.yml.IC -> Datei mit den Icon-Substitutionen (___IC1___;£decision_icon_small;NZL_reward_decision_tt:;513)
					->	_SteamKeysToCreate.yml.NE -> Datei mit den NestedString-Substitutionen (___NE1___;$excavation3$;PER_resource_industry_incompetence_desc:;32)
					->	_SteamKeysToCreate.yml.NS -> Datei mit den NameSpace-Substitutionen (___NS1___;[AUS.GetNameDefCap];AUS_integrated_military_desc:0;4)
 - Die Datei _SteamKeysToCreate.yml.sub in Excel einfügen, Die Übersetzung in der Datei _SteamKeysToCreate.yml.sub.german abspeichern
 - STEAM_RESUB:		->	In die Datei _SteamKeysToCreate.yml.sub.german werden die Substitutionen wieder zurückübersetzt. Es wird eine Datei _SteamKeysToCreate.yml.sub.german.resub erstellt
 - STEAM_INSERT:	->	Fügt die übersetzten Strings in die jeweiligen Dateien ein, legt von den Originaldateien eine Kopie an. Nicht vorhandene Datei werden neu erstellt. (Codierung UTF-8 BOM)

ToDo:
	In der Datei _SteamKeysToCreate.yml.sub die Zeichen '#' maskieren/entfernen. Excel verschiebt sonst die Ende Anführungszeichen nach hinten.
	Nach dem Einfügen (STEAM_INSERT) die Originaldatei sortieren ...

Functions:
        public static readonly string SteamDiff = "STEAM_DIFF";     // public static string FUNCTION_DIFF_STEAM = "diff_steam";
        public static readonly string SteamSub = "STEAM_SUB";       // public static string FUNCTION_SUB_ANALYSE = "sub_analyse";
        public static readonly string SteamResub = "STEAM_RESUB";   // public static string FUNCTION_RESUB_ANALYSE = "resub_analyse";
        public static readonly string SteamInsert = "STEAM_INSERT"; // public static string FUNCTION_INSERT = "insert";
        public static readonly string Diff = "DIFF";                // public static string FUNCTION_DIFF = "diff";
        public static readonly string Sub = "SUB";                  // public static string FUNCTION_SUB = "sub";
        public static readonly string Resub = "RESUB";              // public static string FUNCTION_RESUB = "resub";
        public static readonly string Analyse = "ANALYSE";          // public static string FUNCTION_ANALYSIS = "analyse";
        public static readonly string CheckForDoubleKeys = "CHECK_FOR_DOUBLE_KEYS";
        public static readonly string CheckForDoubleKeysAllFiles = "CHECK_FOR_DOUBLE_KEYS_ALL_FILES";
        public static readonly string CheckForDoubleKeysAllFilesFix = "CHECK_FOR_DOUBLE_KEYS_ALL_FILES_FIX";
