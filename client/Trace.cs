using Oci.GenerativeaiagentruntimeService.Models;

public class Trace
{
    public string TraceType { get; set; }
    public string Key { get; set; }
    public long TimeCreated { get; set; }
    public long TimeFinished { get; set; }
    public int InputCharCount { get; set; }
    public string Input { get; set; }
    public int OutputCharCount { get; set; }
    public string Output { get; set; }
    public string Generation { get; set; }
    public Citation[] Citations { get; set; }
}