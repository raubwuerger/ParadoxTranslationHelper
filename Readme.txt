Ablauf Differenzübersetzung gegen Steamordner:
 - DIFF_FILES:			->	Erstellt Datei _SteamKeysToCreate.yml und _SteamKeysToDelete.yml im Verzeichnis analyse
							Diese Datei auf korrekte strings prüfen ("" am Dateiende, ...)
 - REMOVE_KEYS:			->	Entfernt alle im Steam-Ordner nicht mehr vorhandenen Schlüeesl _SteamKeysToDelete.yml.sub
 - SUB:					->	Erstellt Datei _SteamKeysToCreate.yml.sub (Datei mit Substitutionen)
						->	_SteamKeysToCreate.yml.KY -> Keys-Datei (|___KY000006___|;ARM_purge_the_trotskyist_dashnaks_alt:	;ARM_purge_the_trotskyist_dashnaks_alt:;9)
						->	_SteamKeysToCreate.yml.CC -> ColoCode-Datei (|___CC000010___|;§0;FOCUS_ALTERNATE_ICONS:;352)
						->	_SteamKeysToCreate.yml.IC -> Datei mit den Icon-Substitutionen (|___IC000001___|;£decision_icon_small;NZL_reward_decision_tt:;513)
						->	_SteamKeysToCreate.yml.NE -> Datei mit den NestedString-Substitutionen (|___NE000001___|;$excavation3$;PER_resource_industry_incompetence_desc:;32)
						->	_SteamKeysToCreate.yml.NS -> Datei mit den NameSpace-Substitutionen (|___NS000001___|;[AUS.GetNameDefCap];AUS_integrated_military_desc:0;4)
						->	_SteamKeysToCreate.yml.NL -> Datei mit den NewLine-Substitutionen (|___NL000001___|;[AUS.GetNameDefCap];AUS_integrated_military_desc:0;4)
 - LINE_CORRECOR:		->	In der Datei _SteamKeysToCreate.yml.sub.german werden die Schlüssel zurück substituiert -> Datei _SteamKeysToCreate.yml.sub.german.corrected wird erstellt
 - LINE_CORRECOR_SUB:	->	In der Datei _SteamKeysToCreate.yml.sub.german.corrected werden die übrigen Susbstitutionen ersetzt und in die Datei _SteamKeysToCreate.yml.sub.german.corrected.corrected geschrieben
 - INSERT:				->	Fügt die übersetzten Zeilen in die jeweilige Dateien ein, legt von den Originaldateien eine Kopie an. Nicht vorhandene Datei werden neu erstellt. (Codierung UTF-8 BOM)

=========================================================================================================================================
 - ANALYSE:			->	Vergleicht Keys aus dem Steamordner mit Keys aus dem lokalen Ordner
 - DIFF_KEYS:		->	Vergleicht Keys aus dem Steamordner mit Keys aus dem lokalen Ordner Datei für Datei

ToDo:
	In der Datei _SteamKeysToCreate.yml.sub die Zeichen '#' maskieren/entfernen. Excel verschiebt sonst die Ende Anführungszeichen nach hinten.
	Nach dem Einfügen (INSERT) die Originaldatei sortieren ...
	Prüfen ob Keys in Dateien welche nicht mehr in Steam sind (also gelöscht werden sollen) in anderen Dateien vorhanden sind.

Functions:
        public static readonly string DiffFiles = "DIFF_FILES";
        public static readonly string RemoveKeys = "REMOVE_KEYS";
        public static readonly string Sub = "SUB";
        public static readonly string Resub = "RESUB";
        public static readonly string Insert = "INSERT";
        public static readonly string DiffKeys = "DIFF_KEYS";
        public static readonly string Analyse = "ANALYSE";
        public static readonly string Validate = "VALIDATE";
        public static readonly string CheckForDoubleKeys = "CHECK_FOR_DOUBLE_KEYS";
        public static readonly string CheckForDoubleKeysAllFiles = "CHECK_FOR_DOUBLE_KEYS_ALL_FILES";
        public static readonly string CheckForDoubleKeysAllFilesFix = "CHECK_FOR_DOUBLE_KEYS_ALL_FILES_FIX";
