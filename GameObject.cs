using CUSTOM_PROGRAM_TEST;

public abstract class GameObject
{

    public virtual double X { get; set; }
    public virtual double Y { get; set; }
    public virtual double Width { get; set; }
    public virtual double Height { get; set; }


    // Other properties and methods...

    public virtual bool Intersects(GameObject other)
    {
        
        Console.WriteLine("X: " + X + " Y: " + Y + " Width: " + Width + " Height: " + Height);
        Console.WriteLine("I am called");
        // Define the boundaries of this object
        double thisRightEdge = this.X + this.Width;
        double thisLeftEdge = this.X;
        double thisTopEdge = this.Y;
        double thisBottomEdge = this.Y + this.Height;
        Console.WriteLine("This Right Edge: " + thisRightEdge + " This Left Edge: " + thisLeftEdge + " This Top Edge: " + thisTopEdge + " This Bottom Edge: " + thisBottomEdge);
        // Define the boundaries of the other object
        double otherRightEdge = other.X + other.Width;
        double otherLeftEdge = other.X;
        double otherTopEdge = other.Y;
        double otherBottomEdge = other.Y + other.Height;
        Console.WriteLine("Other Right Edge: " + otherRightEdge + " Other Left Edge: " + otherLeftEdge + " Other Top Edge: " + otherTopEdge + " Other Bottom Edge: " + otherBottomEdge);
        // Check if there is an overlap between this object's and the other object's boundaries
        bool horizontalOverlap = thisRightEdge > otherLeftEdge && thisLeftEdge < otherRightEdge;
        bool verticalOverlap = thisBottomEdge > otherTopEdge && thisTopEdge < otherBottomEdge;

        // Return true if both horizontal and vertical overlaps exist, indicating a collision
        return horizontalOverlap && verticalOverlap;
    }
    // public bool Intersects(Enemy enemy)
    // {
    //     if(_shield)
    //     {
    //         return false;
    //     }
    //     return _x < enemy.X + enemy.Width &&
    //            _x + _sourceRect.Width > enemy.X &&
    //            _y < enemy.Y + enemy.Height &&
    //            _y + _sourceRect.Height > enemy.Y;
    // }
    //
    // public bool Intersects(PowerUp powerUp)
    // {
    //     return _x < powerUp.X + powerUp.Width &&
    //            _x + _sourceRect.Width > powerUp.X &&
    //            _y < powerUp.Y + powerUp.Height &&
    //            _y + _sourceRect.Height > powerUp.Y;
    // }
    
}

