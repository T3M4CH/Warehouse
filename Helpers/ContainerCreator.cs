namespace Warehouse.Helpers;

public static class ContainerCreator
{
    // public static IContainerBase CreateContainer(this Container container)
    // {
    //     return (container.Type, container.Category) switch
    //     {
    //         (EContainerType.Box, EProductType.Animals) => new Box<Animals>(container.MaxWeight),
    //         (EContainerType.Box, EProductType.Food) => new Box<Food>(container.MaxWeight),
    //         (EContainerType.Box, EProductType.Clothes) => new Box<Clothes>(container.MaxWeight),
    //
    //         (EContainerType.Pallet, EProductType.Animals) => new Pallet<Animals>(container.MaxWeight, 25.0),
    //         (EContainerType.Pallet, EProductType.Food) => new Pallet<Food>(container.MaxWeight, 25.0),
    //         (EContainerType.Pallet, EProductType.Clothes) => new Pallet<Clothes>(container.MaxWeight, 25.0),
    //
    //         _ => throw new Exception("Bad container")
    //     };
    // }
}