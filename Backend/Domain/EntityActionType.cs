namespace Backend.Domain
{
    public enum EntityActionType
    {
        CreateLoan = 1,
        CreateBorrower,
        SetLoanField,
        SetBorrowerField,
        Unsupported = 99
    }
}
