
using Telerik.Blazor.Components;

namespace AplixacionPrueba3.Pages;

public partial class GanttComponente2
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Title { get; set; }
    public double PercentComplete { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public int LastId { get; set; } = 1;

    public string TreeListWidth { get; set; } = "50%";

    List<GanttComponente2> Data { get; set; }

    private GanttComponente2 GetParent(GanttComponente2 item)
    {
        return Data.FirstOrDefault(i => i.Id.Equals(item.ParentId));
    }

    private IEnumerable<GanttComponente2> GetChildren(GanttComponente2 item)
    {
        return Data.Where(i => item.Id.Equals(i.ParentId));
    }

    protected override void OnInitialized()
    {
        Data = new List<GanttComponente2>();

        var random = new Random();

        for (int i = 1; i < 1; i++)
        {
            var newItem = new GanttComponente2()
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
                Data.Add(new GanttComponente2()
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
        var argsItem = args.Item as GanttComponente2;

        argsItem.Id = Data.Count + 100;

        if (args.ParentItem != null)
        {
            var parent = (GanttComponente2)args.ParentItem;

            argsItem.ParentId = parent.Id;
        }

        Data.Insert(0, argsItem);

        CalculateParentPercentRecursive(argsItem);
        CalculateParentRangeRecursive(argsItem);
    }

    private void RemoveChildRecursive(GanttComponente2 item)
    {
        var children = GetChildren(item).ToList();

        foreach (var child in children)
        {
            RemoveChildRecursive(child);
        }

        Data.Remove(item);
    }

    private void CalculateParentPercentRecursive(GanttComponente2 item)
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

    private void CalculateParentRangeRecursive(GanttComponente2 item)
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

    private void MoveChildrenRecursive(GanttComponente2 item, TimeSpan offset)
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
