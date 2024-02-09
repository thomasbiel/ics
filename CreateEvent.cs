using System;
using System.Globalization;
using System.IO;
using CommandLine;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Calendar = Ical.Net.Calendar;

namespace ical;

[Verb("create")]
public class CreateEvent
{
    [Option('s', "start", Required = true)]
    public DateTime Start { get; set; }
    
    [Option('e', "end", Group = "time")]
    public DateTime End { get; set; }
    
    [Option('d', "duration", Group = "time")]
    public string Duration { get; set; }
    
    [Option('l', "location")]
    public string Location { get; set; }
    
    [Option('m', "summary")]
    public string Summary { get; set; }
    
    [Option('r', "description")]
    public string Description { get; set; }
    
    [Option('f', "filename")]
    public string Filename { get; set; }

    public void Save()
    {
        // Create a new calendar
        var calendar = new Calendar();

        DateTime end;
        if (this.End != DateTime.MinValue)
        {
            end = this.End;
        }
        else
        {
            end = this.Start + TimeSpan.Parse(this.Duration);
        }

        // Create a new event
        var evt = new CalendarEvent
        {
            Summary = this.Summary,
            Description = this.Description,
            Start = new CalDateTime(this.Start),
            End = new CalDateTime(end),
            Location = this.Location
        };

        // Add the event to the calendar
        calendar.Events.Add(evt);

        // Write the calendar to a file
        var serializer = new CalendarSerializer();
        var serializedCalendar = serializer.SerializeToString(calendar);
        File.WriteAllText(Path.ChangeExtension(this.Filename, ".ics"), serializedCalendar);
    }
}