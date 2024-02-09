using System;
using System.Globalization;
using System.IO;
using CommandLine;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Calendar = Ical.Net.Calendar;

namespace ical;

[Verb("create")]
public class CreateEvent
{
    [Option('s', "start", Required = true, HelpText = "The start date and time of the event")]
    public string Start { get; set; }
    
    [Option('e', "end", Group = "time", HelpText = "The end date and time of the event")]
    public string End { get; set; }
    
    [Option('d', "duration", Group = "time", HelpText = "The duration of the event")]
    public string Duration { get; set; }
    
    [Option('l', "location", HelpText = "The location of the event")]
    public string Location { get; set; }
    
    [Option('m', "summary", HelpText = "The summary of the event")]
    public string Summary { get; set; }
    
    [Option('r', "description", HelpText = "The description of the event")]
    public string Description { get; set; }
    
    [Option('f', "filename", HelpText = "The name of the file to save the event to")]
    public string Filename { get; set; }

    [Option('a', "alarm", Default = 15, HelpText = "Minutes before the event to trigger the alarm, 0 = no alarm")]
    public int Alarm { get; set; }

    public void Save()
    {
        // Create a new calendar
        var calendar = new Calendar();

        var start = DateTime.Parse(this.Start);
        
        DateTime end;
        if (this.End != null)
        {
            end = DateTime.Parse(this.End);
        }
        else
        {
            end = start + TimeSpan.Parse(this.Duration);
        }

        // Create a new event
        var calendarEvent = new CalendarEvent
        {
            Summary = this.Summary,
            Description = this.Description,
            Start = new CalDateTime(start, TimeZoneInfo.Local.Id),//"Europe/Berlin"),
            End = new CalDateTime(end),
            Location = this.Location
        };
        
        if (this.Alarm > 0)
        {
            var alarm = new Alarm
            {
                Action = AlarmAction.Display,
                Trigger = new Trigger(TimeSpan.FromMinutes(this.Alarm * -1))
            };
            
            calendarEvent.Alarms.Add(alarm);
        };

        // Add the event to the calendar
        calendar.Events.Add(calendarEvent);

        // Write the calendar to a file
        var serializer = new CalendarSerializer();
        var serializedCalendar = serializer.SerializeToString(calendar);
        File.WriteAllText(Path.ChangeExtension(this.Filename, ".ics"), serializedCalendar);
    }
}