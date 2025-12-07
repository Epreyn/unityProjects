public enum StatModType {
    Flat = 100,
    PercentAdd = 200,
    PercentMult = 300
}

public class StatModifier {

    public readonly string Name;
    public readonly int Value;
    public readonly StatModType Type;
    public readonly int Order;
    public readonly object Source;

    public StatModifier(string name, int value, StatModType type, int order = -1, object source = null) {
        Name = name;
        Value = value;
        Type = type;
        Order = order == -1 ? (int)type : order;
        Source = source;
    }

    public StatModifier(string name, int value, StatModType type) : this(name, value, type, (int) type, null) { }
    
    public StatModifier(string name, int value, StatModType type, int order) : this(name, value, type, order, null) { }
    
    public StatModifier(string name, int value, StatModType type, object source) : this(name, value, type, (int) type, source) { }
}