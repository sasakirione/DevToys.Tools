using System.Globalization;
using Bogus;

namespace DevToys.Tools.Helpers;

internal static class FakeHelper
{
    private static Faker faker;

    static FakeHelper()
    {
        faker = new Faker();
    }

    internal static void SetLocale(string dotNetLocale)
    {
        faker = new Faker(GetBogusCulture(dotNetLocale));
    }

    private static string GetBogusCulture(string dotNetLocale)
    {
        string locale = dotNetLocale.Replace("-", "_");
        if (Database.LocaleResourceExists(locale))
        {
            return locale;
        }

        string shortLocale = locale.Split('_')[0];
        return Database.LocaleResourceExists(shortLocale) ? shortLocale : "en";
    }

    public static string GetPhoneNumber()
    {
        return faker.Phone.PhoneNumber();
    }

    public static string GetEmailAddress()
    {
        return faker.Internet.Email();
    }
}

internal enum FakeType
{
    PhoneNumber,
    MailAddress
}
