namespace PetShelter.Data.Enums;

/// <summary>Пол на животното.</summary>
public enum Gender
{
    Unknown = 0,
    Male = 1,
    Female = 2
}

/// <summary>Размер на животното — използва се във филтрите на каталога.</summary>
public enum AnimalSize
{
    Small = 1,
    Medium = 2,
    Large = 3,
    ExtraLarge = 4
}

/// <summary>Статус на животно в приюта.</summary>
public enum AnimalStatus
{
    Available = 1,
    Reserved = 2,
    Adopted = 3
}

/// <summary>Статус на заявка за осиновяване.</summary>
public enum AdoptionRequestStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Cancelled = 4
}

/// <summary>Статус на сигнал за бездомно животно.</summary>
public enum StrayReportStatus
{
    New = 1,
    InProgress = 2,
    Resolved = 3,
    Rejected = 4
}
