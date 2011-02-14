# DynoPhile - A dynamic, ExpandoObject-o-matic, file-to-poco parser, yeah.

The idea is simple. Take a CSV file, sprinkle some dynamic and ExpandoObject magic, and get a list of plain old object back.

Warning - this is a toy and experimental code. Perhaps it will turn into something, perhaps it will not. Who knows.

## Usage:
First, add a reference to DynoPhile.dll  
For a file with a header: make the call:  
<pre>
var magic = new DynoPhile().ReadFile(fileName, ",");  
</pre>

for a file without a header, you can supply your own function to build out the dynamic properties:  
<pre>  
public class DoMagic()
{
	var magic = new DynoPhile().ReadFile(fileName, "|", BuildProperties);
}
		
private static dynamic BuildProperties()
{
	dynamic header = new ExpandoObject();

	header.FirstName = string.Empty;
	header.LastName = string.Empty;
	header.TwitterHandle = string.Empty;

	return header;
}
</pre>

so given an example CSV file:

<pre>
"FirstName","LastName","Twitter"
"Jeff","Schumacher","codereflection"
"Bobby","Johnson","notmyself"
"Eric","Ridgeway","ang3lfir3"
</pre>

DynoPhile will give you dynamic awesome in return:

<pre>
foreach (var item in magic)
{
	Console.WriteLine("{0} {1}, @{2}", item.FirstName, item.LastName, item.Twitter);
}
</pre>