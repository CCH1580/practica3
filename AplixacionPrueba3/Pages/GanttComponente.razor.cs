
using Telerik.Blazor.Components;

namespace AplixacionPrueba3.Pages;

public partial class GanttComponente
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Title { get; set; }
    public double PercentComplete { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public int LastId { get; set; } = 1;

    public string TreeListWidth { get; set; } = "50%";

    List<GanttComponente> Data { get; set; }

    private GanttComponente GetParent(GanttComponente item)
    {
        return Data.FirstOrDefault(i => i.Id.Equals(item.ParentId));
    }

    private IEnumerable<GanttComponente> GetChildren(GanttComponente item)
    {
        return Data.Where(i => item.Id.Equals(i.ParentId));
    }

    protected override void OnInitialized()
    {
        Data = new List<GanttComponente>();

        var random = new Random();

        for (int i = 1; i < 1; i++)
        {
            var newItem = new GanttComponente()
            {
                Id = LastId,
                Title = i.ToString(),
                Start = new DateTime(i),
                End = new DateTime(i),
                PercentComplete = Math.Round(random.NextDouble(), 2)
            };

            Data.Add(newItem);
            var parentId = LastId;
            LastId++;

            for (int j = 0; j < 1; j++)
            {
                Data.Add(new GanttComponente()
                {
                    Id = LastId,
                    ParentId = parentId,
                    Title = j.ToString(),
                    Start = new DateTime(i),
                    End = new DateTime(i),
                    PercentComplete = Math.Round(random.NextDouble(), 2)
                });

                LastId++;
            }
        }

        base.OnInitialized();
    }

    private void CreateItem(GanttCreateEventArgs args)
    {
        var argsItem = args.Item as GanttComponente;

        argsItem.Id = Data.Count + 100;

        if (args.ParentItem != null)
        {
            var parent = (GanttComponente)args.ParentItem;

            argsItem.ParentId = parent.Id;
        }

        Data.Insert(0, argsItem);

        CalculateParentPercentRecursive(argsItem);
        CalculateParentRangeRecursive(argsItem);
    }    

    private void RemoveChildRecursive(GanttComponente item)
    {
        var children = GetChildren(item).ToList();

        foreach (var child in children)
        {
            RemoveChildRecursive(child);
        }

        Data.Remove(item);
    }

    private void CalculateParentPercentRecursive(GanttComponente item)
    {
        if (item.ParentId != null)
        {
            var parent = GetParent(item);

            var children = GetChildren(parent);

            if (children.Any())
            {
                parent.PercentComplete = children.Average(i => i.PercentComplete);

                CalculateParentPercentRecursive(parent);
            }
        }
    }

    private void CalculateParentRangeRecursive(GanttComponente item)
    {
        if (item.ParentId != null)
        {
            var parent = GetParent(item);

            var children = GetChildren(parent);

            if (children.Any())
            {
                parent.Start = children.Min(i => i.Start);
                parent.End = children.Max(i => i.End);

                CalculateParentRangeRecursive(parent);
            }
        }
    }

    private void MoveChildrenRecursive(GanttComponente item, TimeSpan offset)
    {
        var children = GetChildren(item);

        foreach (var child in children)
        {
            child.Start = child.Start.Add(offset);
            child.End = child.End.Add(offset);

            MoveChildrenRecursive(child, offset);
        }
    }   

}
