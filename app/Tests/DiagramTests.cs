using System.Collections.Generic;
using System.Linq;
using AutoUml;
using Xunit;

namespace Tests
{
    public class DiagramTests : TestsBase
    {
        [Fact]
        public void T01_Should_create_simple_diagram()
        {
            var b = new ReflectionProjectBuilder(true)
                .WithAssembly(typeof(DiagramTests).Assembly)
                .Build();
            Assert.NotNull(b);
            Assert.True(b.Diagrams.ContainsKey("Test"));
            var diag = b.Diagrams["Test"];
            Assert.NotNull(diag);

            var file = diag.CreateFile();
            Assert.NotNull(file);
            var code = file.Code;
            Save(code);

            var expected = @"@startuml
title
 Diagram Test
end title

class Order #ff0000
{
}
class OrderItem
{
}

Order --{ OrderItem:Items
@enduml
";
            Assert.Equal(expected, code);
        }


        [Fact]
        public void T02_Should_create_simple_diagram_with_entity_note()
        {
            var b = new ReflectionProjectBuilder(true)
                .WithAssembly(typeof(DiagramTests).Assembly)
                .Build();
            Assert.NotNull(b);
            Assert.True(b.Diagrams.ContainsKey("Test"));
            var diag = b.Diagrams["Test"];
            Assert.NotNull(diag);

            var ent = diag.GetEntityByType(typeof(Order));
            ent.AddNote(NoteLocation.Top, "Sample note");

            var file = diag.CreateFile();
            Assert.NotNull(file);
            var code = file.Code;
            Save(code);
            // 1074296
            var expected = @"@startuml
title
 Diagram Test
end title

class Order #ff0000
{
}
note top of Order

Sample note
end note
class OrderItem
{
}

Order --{ OrderItem:Items
@enduml
";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T03_Should_create_simple_diagram_with_auto_entity_note()
        {
            var b = new ReflectionProjectBuilder(true)
                .WithAssembly(typeof(DiagramTests).Assembly)
                .Build();
            Assert.NotNull(b);
            Assert.True(b.Diagrams.ContainsKey("Test2"));
            var diag = b.Diagrams["Test2"];
            Assert.NotNull(diag);

            var file = diag.CreateFile();
            Assert.NotNull(file);
            var code = file.Code;
            Save(code);
            var expected = @"@startuml
title
 Diagram Test2
end title

class Order2
{
}
note right of Order2

Note from annotation
end note
class OrderItem2
{
}
note left of OrderItem2

Note from UmlNote
end note

Order2 --{ OrderItem2:Items
@enduml
";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T04_Should_create_simple_diagram_with_entity_note_with_background()
        {
            var b = new ReflectionProjectBuilder(true)
                .WithAssembly(typeof(DiagramTests).Assembly)
                .Build();
            Assert.NotNull(b);
            Assert.True(b.Diagrams.ContainsKey("Test"));
            var diag = b.Diagrams["Test"];
            Assert.NotNull(diag);

            var ent = diag.GetEntityByType(typeof(Order));
            ent.AddNote(NoteLocation.Top, "Sample note", UmlColor.IndianRed.ToFill());

            var file = diag.CreateFile();
            Assert.NotNull(file);
            var code = file.Code;
            Save(code);
            // 1074296
            var expected = @"@startuml
title
 Diagram Test
end title

class Order #ff0000
{
}
note top of Order #indianred

Sample note
end note
class OrderItem
{
}

Order --{ OrderItem:Items
@enduml
";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T06_Should_create_note_on_relation()
        {
            var b = new ReflectionProjectBuilder(true)
                .WithAssembly(typeof(DiagramTests).Assembly)
                .Build();
            Assert.NotNull(b);
            Assert.True(b.Diagrams.ContainsKey("Test"));
            var diag = b.Diagrams["Test"];
            Assert.NotNull(diag);
            var rel = diag.Relations.Single();
            rel.Note = "Note on rel";
            rel.NoteBackground = new GradientColorFill(UmlColor.Aqua, UmlColor.AliceBlue, GradientDirection.DownRight); 

            var file = diag.CreateFile();
            Assert.NotNull(file);
            var code = file.Code;
            Save(code);

            var expected = @"@startuml
title
 Diagram Test
end title

class Order #ff0000
{
}
class OrderItem
{
}

Order --{ OrderItem:Items
note on link  #aqua/aliceblue
Note on rel
end note
@enduml
";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T05_Should_create_spot_with_background()
        {
            var b = new ReflectionProjectBuilder(true)
                .WithAssembly(typeof(DiagramTests).Assembly)
                .Build();
            Assert.NotNull(b);
            Assert.True(b.Diagrams.ContainsKey("Test"));
            var diag = b.Diagrams["Test"];
            Assert.NotNull(diag);

            var ent = diag.GetEntityByType(typeof(Order));
            ent.Spot = new UmlSpot
            {
                InCircle              = "X",
                CircleBackgroundColor = UmlColor.Blue
            };

            var file = diag.CreateFile();
            Assert.NotNull(file);
            var code = file.Code;
            Save(code);
            var expected = @"@startuml
title
 Diagram Test
end title

class Order << (X,#0000ff) >> #ff0000
{
}
class OrderItem
{
}

Order --{ OrderItem:Items
@enduml
";
            Assert.Equal(expected, code);
        }
    }


    [UmlDiagram("Test", BackgroundColor = "ff0000")]
    public class Order
    {
        public List<OrderItem> Items { get; set; }
    }

    [UmlDiagram("Test")]
    public class OrderItem
    {
    }


    [UmlDiagram("Test2", Note = "Note from annotation", NoteLocation = NoteLocation.Right)]
    public class Order2
    {
        public List<OrderItem2> Items { get; set; }
    }

    [UmlDiagram("Test2")]
    [UmlNote("Note from UmlNote", NoteLocation = NoteLocation.Left)]
    public class OrderItem2
    {
    }
}