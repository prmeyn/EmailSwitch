using HumanLanguages;

namespace EmailSwitch.Translations
{
	public static class TranslationService
	{
		public static string[] GetTranslation(this TranslationKey key, LanguageIsoCode languageIsoCode)
		{
			if (TranslationsDictionary.TryGetValue(key, out var keyTranslations) &&
				keyTranslations.TryGetValue(languageIsoCode.LanguageId, out var translations))
			{
				var translation = translations.GetValueOrDefault(languageIsoCode.LanguageLocaleVariationCode);
				return translation is null || translation.Length == 0 ? translations.GetValueOrDefault(LanguageLocaleVariationCode.Default) : translation;
			}

			// Handle the case where the translation is not found.
			return Array.Empty<string>();
		}

		public static Dictionary<TranslationKey, Dictionary<LanguageId, Dictionary<LanguageLocaleVariationCode, string[]>>> TranslationsDictionary = new()
		{
			{
				TranslationKey.Test,
				new Dictionary<LanguageId, Dictionary<LanguageLocaleVariationCode, string[]>>()
				{
					{
						LanguageId.en,
						new Dictionary<LanguageLocaleVariationCode, string[]>()
						{
							{ LanguageLocaleVariationCode.Default, new string[] { "Test", "Colour" } },
							{ LanguageLocaleVariationCode.US, new string[] { "Test", "Color" } }
						}
					},
					{
						LanguageId.da,
						new Dictionary<LanguageLocaleVariationCode, string[]>()
						{
							{ LanguageLocaleVariationCode.Default, new string[] { "Test", "farve" } }
						}
					}
				}
			}
		};

	}
}
