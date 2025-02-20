Ablauf Differenzübersetzung gegen Steamordner:
 - STEAM_DIFF:		->	Erstellt Datei MissingTranslationKeysSteam.yml im Verzeichnis analyse
					Diese Datei auf korrekte strings prüfen ("" am Dateiende, ...)
 - STEAM_SUB:		->	Erstellt Datei MissingTranslationKeysSteam.yml.sub (Datei mit Substitutionen)
					->	MissingTranslationKeysSteam.yml.CC -> ColoCode-Datei (aktuell leer da nur die ColorCode-EndeTags übersetzt werden)
					->	MissingTranslationKeysSteam.yml.IC -> Datei mit den Icon-Substitutionen (___IC1___;£decision_icon_small;NZL_reward_decision_tt:;513)
					->	MissingTranslationKeysSteam.yml.NE -> Datei mit den NestedString-Substitutionen (___NE1___;$excavation3$;PER_resource_industry_incompetence_desc:;32)
					->	MissingTranslationKeysSteam.yml.NS -> Datei mit den NameSpace-Substitutionen (___NS1___;[AUS.GetNameDefCap];AUS_integrated_military_desc:0;4)
 - Die Datei MissingTranslationKeysSteam.yml.sub in Excel einfügen, Die Übersetzung in der Datei MissingTranslationKeysSteam.yml.sub.german abspeichern
 - STEAM_RESUB:		->	In die Datei MissingTranslationKeysSteam.yml.sub.german werden die Substitutionen wieder zurückübersetzt. Es wird eine Datei MissingTranslationKeysSteam.yml.sub.german.resub erstellt
 - STEAM_INSERT:	->	Fügt die übersetzten Strings in die jeweiligen Dateien ein, legt von den Originaldateien eine Kopie an. Nicht vorhandene Datei werden neu erstellt. (Codierung UTF-8 BOM)

ToDo:
	Nach dem Einfügen (STEAM_INSERT) die Originaldatei sortieren ...

