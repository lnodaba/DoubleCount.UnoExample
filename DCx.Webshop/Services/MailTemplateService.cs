using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Components;
using DCx.Webshop.Pages;

using DCx.Webshop.Models;

namespace DCx.Webshop.Services
{
    public static class MailTemplateService
    {
        private static string Language
        {
            get => System.Globalization.CultureInfo.CurrentCulture.Name;
        }


        public static string GetOrderEmail(ProductItem product, Invoice invoice, License license, IStringLocalizer<Licenses> localizer) => Language switch
        {
            "de" => GetGermanOrderEmail(product, invoice, license, localizer),
            _ => GetEnglishOrderEmail(product, invoice, license, localizer)
        };

        private static string GetEnglishOrderEmail(ProductItem product, Invoice invoice, License license, IStringLocalizer<Licenses> localizer) =>
@$" Dear {invoice.FirstName} { invoice.LastName},

Thank you for the received order, which will be executed as soon as possible.
In a first step you will receive an invoice, the license will be submitted after payment has been received.

In case you like to use the payroll, you may register a company with a valid UID Number as a Rental - License.
This can be done in -Register Stammdaten, Firma - and will unlocked in interim mode.
Within 45 days you can use the application without limitiations.

On our website www.doublecount.ch you find information and help that assist you with double count and dialogik products.

If you have any question, our support is happy to help.

Best regards,
double count

{ProductTemplate(product, invoice, license, localizer)}
";

        private static string GetGermanOrderEmail(ProductItem product, Invoice invoice, License license, IStringLocalizer<Licenses> localizer) =>
$@"Sehr geehrte(r)  {invoice.FirstName} { invoice.LastName}

Vielen Dank für Ihre Bestellung, welche wir baldmöglichst ausführen.
Sie werden von uns zuerst eine Rechnung erhalten, die Lizenz wird nach Zahlungseingang zugestellt.

Möchten Sie in der Zwischenzeit bereits Lohnmandanten bearbeiten, können Sie im Firmenstammblatt - Register Stammdaten, Firma - mit Angabe der UID Nummer einen Mandantenlizenz im Interim-Modus freischalten.
Diese bleibt für 45 Tage gültig und ermöglicht Ihnen das Lohnprogramm während dieser Zeit uneingeschränkt zu nutzen.

Auf unserer Website www.doublecount.ch finden Sie Hinweise zur Verwendung der double count und dialogik Produkte.

In jedem Fall sind wir für Sie da: für Fragen steht Ihnen der Support gerne zur Verfügung.

Freundliche Grüsse
double count
            
{ProductTemplate(product, invoice, license, localizer)}
";

        private static string ProductTemplate(ProductItem product, Invoice invoice, License license, IStringLocalizer<Licenses> localizer)
        {
            IStringLocalizer<Licenses> _localizer = localizer;
            return
@$"-------------{_localizer["Product"]}-------------------
{_localizer[product.Name]} {product.Quantity.Value} {_localizer[product.Hosting.Name]}
{string.Join("\n", product.Prices.Select(x => $"{localizer[x.licenseOption.Name]} {x.ProRata}"))}
-------------{_localizer["License"]}-------------------
{license.Email}
{license.Company}
{license.FirstName} {license.LastName}
{license.Address}
{license.Zip} {license.City}
-------------{_localizer["Invoice"]}-------------------
{invoice.Email}
{invoice.Company}
{invoice.FirstName} {invoice.LastName}
{invoice.Address}
{invoice.Zip} {invoice.City}
---------------------------------------";
        }
        public static string GetTicketCreationEmail(Ticket ticket, License license, MailLinks mailLinks/* ,IStringLocalizer<Tickets> localizer*/)
            => Language switch
            {
                _ => GetEnglishTicketCreationEmail(ticket, license, mailLinks)
            };

        private static string GetEnglishTicketCreationEmail(Ticket ticket, License license, MailLinks mailLinks) =>
@$"Dear {license.FirstName} { license.LastName} 

New ticket was created on the {ticket.Product.Name} product.
The ticket details are:
    Subject: {ticket.Subject}
    Description: {ticket.Communications.FirstOrDefault()?.Description} 
    On: {ticket.Communications.FirstOrDefault()?.CreatedDate}

{mailLinks.AttachmentLink}

Open your ticket here: {mailLinks.TicketLink}

Best regards,
double count";

        public static string GetTicketCommentEmail(Ticket ticket, License license, Communication communication, MailLinks mailLinks/* ,IStringLocalizer<Tickets> localizer*/)
         => Language switch
         {
             _ => GetEnglishTicketCommentEmail(ticket, license, communication, mailLinks)
         };

        private static string GetEnglishTicketCommentEmail(Ticket ticket, License license, Communication communication, MailLinks mailLinks) =>
@$"Dear {license.FirstName} { license.LastName} 

New comment was added on ticket {ticket.Subject}.
The comment details are:
    CreatedBy: {communication?.CreatedBy}
    On: {communication?.CreatedDate}
    Description: {communication?.Description}

{mailLinks.AttachmentLink}

Open your ticket here: {mailLinks.TicketLink}

Best regards,
double count";


        public static string GetTicketCreatedFromEmail(Ticket ticket, MailLinks mailLinks/* ,IStringLocalizer<Tickets> localizer*/)
            => Language switch
            {
                _ => GetEnglishTicketCreatedFromEmail(ticket, mailLinks)
            };

        private static string GetEnglishTicketCreatedFromEmail(Ticket ticket, MailLinks mailLinks) =>
@$"Dear {ticket.CreatedBy} 

Your ticket was created.
The ticket details are:
    Subject: {ticket.Subject}
    Description: {ticket.Communications.FirstOrDefault()?.Description} 
    On: {ticket.Communications.FirstOrDefault()?.CreatedDate}

{mailLinks.AttachmentLink}

Open your ticket here: {mailLinks.TicketLink}

Best regards,
double count";

        public static string GetTicketCommentFromEmail(Ticket ticket, Communication communication, MailLinks mailLinks/* ,IStringLocalizer<Tickets> localizer*/)
         => Language switch
         {
             _ => GetEnglishTicketCommentFromEmail(ticket, communication, mailLinks)
         };

        private static string GetEnglishTicketCommentFromEmail(Ticket ticket, Communication communication, MailLinks mailLinks) =>
@$"Dear {ticket.CreatedBy} 

New comment was added on ticket {ticket.Subject}.
The comment details are:
    CreatedBy: {communication?.CreatedBy}
    On: {communication?.CreatedDate}
    Description: {communication?.Description}

{mailLinks.AttachmentLink}

Open your ticket here: {mailLinks.TicketLink}

Best regards,
double count";

    }
}
