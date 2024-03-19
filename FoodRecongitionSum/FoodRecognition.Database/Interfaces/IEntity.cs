using System.ComponentModel.DataAnnotations.Schema;

namespace FoodRecognition.Database.Interfaces;

public interface IEntity
{
    string Id { get; set; }

    DateTime Created { get; set; }

    DateTime Modified { get; set; }

    bool Deleted { get; set; }
}