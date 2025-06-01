using AnTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnTest.Services
{
    public class EmailProcessing
    {
        private static readonly List<string> Domains = new List<string>
        {
            "tbank.ru",
            "alfa.com",
            "vtb.ru"
        };

        public static readonly Dictionary<string, List<string>> DomainExceptions = new Dictionary<string, List<string>>
        {
            { "tbank.ru", new List<string> { "i.ivanov@tbank.ru" } },
            { "alfa.com", new List<string> { "s.sergeev@alfa.com", "a.andreev@alfa.com" } },
            { "vtb.ru", new List<string>() }
        };

        private static readonly Dictionary<string, List<string>> DomainSubstitutionAddresses = new Dictionary<string, List<string>>
        {
            { "tbank.ru", new List<string> { "t.tbankovich@tbank.ru", "v.veronickovna@tbank.ru" } },
            { "alfa.com", new List<string> { "v.vladislavovich@alfa.com" } },
            { "vtb.ru", new List<string> { "a.aleksandrov@vtb.ru" } }
        };

        private static List<string> ParseEmail(string emailList)
        {
            if (string.IsNullOrEmpty(emailList)) return new List<string>();

            return emailList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(addr => addr.Trim())
                .Where(addr => !string.IsNullOrEmpty(addr))
                .ToList();
        }

        private static bool HasException(IEnumerable<string> addresses)
        {
            return addresses.Any(addr =>
                DomainExceptions.Any(kv =>
                    kv.Value.Contains(addr) &&
                    addr.EndsWith("@" + kv.Key)));
        }

        private static bool IsSubtitions(string address)
        {
            return DomainSubstitutionAddresses.Any(kv =>
                kv.Value.Contains(address) &&
                address.EndsWith("@" + kv.Key));
        }

        private static string FormatEmailList(IEnumerable<string> addresses)
        {
            return string.Join(";", addresses) + (addresses.Any() ? ";" : "");
        }
        public static string ProcessEmailCopy(EmailModel email)
        {
            var toAddresses = ParseEmail(email.To);
            var copyAddresses = ParseEmail(email.Copy);

            bool hasException = HasException(toAddresses.Concat(copyAddresses));

            var domainInToAndCopy = toAddresses.Concat(copyAddresses)
                .Select(addr => addr.Split('@').Last())
                .Where(domain => Domains.Contains(domain))
                .Distinct()
                .ToList();

            if (hasException)
            {
                var filteredCopy = copyAddresses
                    .Where(addr => !IsSubtitions(addr))
                    .ToList();

                return FormatEmailList(filteredCopy);
            }

            else if (domainInToAndCopy.Any()) 
            {
                var resultCopy = new List<string>(copyAddresses);

                foreach (var domain in domainInToAndCopy)
                {
                    foreach (var addr in DomainSubstitutionAddresses[domain])
                    {
                        if (!resultCopy.Contains(addr))
                        {
                            resultCopy.Add(addr);
                        }
                    }
                }
                return FormatEmailList(resultCopy);
            }
            //var hasBussinessDomain = toAddresses.Concat(copyAddresses)
            //    .Any(addr => Domains.Any(domain => addr.EndsWith("@" + domain)) && 
            //    !HasException(toAddresses.Concat(copyAddresses)));

            return FormatEmailList(copyAddresses);


        }
    }
}
