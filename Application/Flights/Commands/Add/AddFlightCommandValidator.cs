using Application.Common.Validators;

namespace Application.Flights.Commands.Add;

public class AddFlightCommandValidator : InputValidator<AddFlightCommand>
{
    public AddFlightCommandValidator()
    {
        //InputValidator и BusinessValidator обычно используем не для crud операций над сущностями.
        //Сущность домена должны сама отвечать за свой инвариант (валидное состояние).
        //Валидаторы используем для запуска бизнес логики не связанной с crud.
    }
}
