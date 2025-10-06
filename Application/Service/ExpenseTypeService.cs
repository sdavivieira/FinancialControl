using FinancialControl.Application.Interface;
using FinancialControl.Domain.Interfaces.Expenses;
using FinancialControl.Domain.Models;
using FinancialControl.ResponseRequest;
using FinancialControl.ResponseRequest.Request.Expense;
using FinancialControl.ResponseRequest.Response.Expense;

namespace FinancialControl.Application.Service
{
    public class ExpenseTypeService : IExpenseTypeService
    {
        private readonly IExpenseTypeWriteRepository _expenseTypeWriteRepository;
        private readonly IExpenseTypeReadRepository _expenseTypeReadRepository;
        private readonly IExpenseWriteRepository _expenseWriteRepository; 
        private readonly IExpenseReadRepository _expenseReadRepository; 

        public ExpenseTypeService(
            IExpenseTypeWriteRepository expenseTypeWriteRepository,
            IExpenseTypeReadRepository expenseTypeReadRepository,
            IExpenseWriteRepository expenseWriteRepository,     
            IExpenseReadRepository expenseReadRepository)       
        {
            _expenseTypeWriteRepository = expenseTypeWriteRepository;
            _expenseTypeReadRepository = expenseTypeReadRepository;
            _expenseWriteRepository = expenseWriteRepository;   
            _expenseReadRepository = expenseReadRepository;     
        }


        public async Task<OperationResult<ExpenseTypeResponse>> Create(ExpenseTypeRequest expense)
        {
            try
            {
                var newExpense = new ExpenseType
                {
                    Name = expense.Name,
                    InicialValue = expense.InicialValue,
                    IsFixed = expense.IsFixed
                };

                await _expenseTypeWriteRepository.Add(newExpense);

                var response = new ExpenseTypeResponse
                {
                    Id = newExpense.Id,
                    Name = newExpense.Name,
                    InicialValue = newExpense.InicialValue,
                    IsFixed = newExpense.IsFixed
                };

                return new OperationResult<ExpenseTypeResponse>
                {
                    Success = true,
                    Message = "Novo registro cadastrado",
                    Data = response
                };
            }
            catch
            {
                return new OperationResult<ExpenseTypeResponse>
                {
                    Success = false,
                    Message = "Erro ao realizar registro!",
                    Data = null
                };
            }
        }

        public async Task<OperationResult<IEnumerable<ExpenseTypeResponse>>> ExpenseTypes()
        {
            var result = await _expenseTypeReadRepository.GetAllAsync();

            if (!result.Any())
                return new OperationResult<IEnumerable<ExpenseTypeResponse>>
                {
                    Success = false,
                    Message = "Não encontrado registro",
                    Data = new List<ExpenseTypeResponse>()
                };

            var response = result.Select(x => new ExpenseTypeResponse
            {
                Id = x.Id,
                Name = x.Name,
                InicialValue = x.InicialValue,
                IsFixed = x.IsFixed
            });

            return new OperationResult<IEnumerable<ExpenseTypeResponse>>
            {
                Success = true,
                Message = "Registros encontrados",
                Data = response
            };
        }

        public async Task<OperationResult<ExpenseTypeResponse>> Update(int id, ExpenseTypeRequest expense)
        {
            try
            {
                var existing = (await _expenseTypeReadRepository.GetAllAsync())
                                .FirstOrDefault(x => x.Id == id);

                if (existing == null)
                    return new OperationResult<ExpenseTypeResponse>
                    {
                        Success = false,
                        Message = "Tipo de despesa não encontrado",
                        Data = null
                    };

                existing.Name = expense.Name;
                existing.InicialValue = expense.InicialValue;
                existing.IsFixed = expense.IsFixed;

                await _expenseTypeWriteRepository.Update(existing);

                var response = new ExpenseTypeResponse
                {
                    Id = existing.Id,
                    Name = existing.Name,
                    InicialValue = existing.InicialValue,
                    IsFixed = existing.IsFixed
                };

                return new OperationResult<ExpenseTypeResponse>
                {
                    Success = true,
                    Message = "Tipo de despesa atualizado com sucesso",
                    Data = response
                };
            }
            catch
            {
                return new OperationResult<ExpenseTypeResponse>
                {
                    Success = false,
                    Message = "Erro ao atualizar tipo de despesa",
                    Data = null
                };
            }
        }

        public async Task<OperationResult<bool>> Delete(int id)
        {
            try
            {
                // Verifica se o ExpenseType existe
                var existingType = (await _expenseTypeReadRepository.GetAllAsync())
                                    .FirstOrDefault(x => x.Id == id);

                if (existingType == null)
                    return new OperationResult<bool>
                    {
                        Success = false,
                        Message = "Tipo de despesa não encontrado",
                        Data = false
                    };

                // Busca todos os Expenses relacionados a esse tipo
                var relatedExpenses = (await _expenseReadRepository.GetAllAsync())
                                        .Where(e => e.ExpenseTypeId == id);

                // Deleta todos os Expenses relacionados
                foreach (var expense in relatedExpenses)
                {
                    await _expenseWriteRepository.Delete(expense);
                }

                // Deleta o ExpenseType
                await _expenseTypeWriteRepository.Delete(existingType);

                return new OperationResult<bool>
                {
                    Success = true,
                    Message = "Tipo de despesa e todos os gastos relacionados deletados com sucesso",
                    Data = true
                };
            }
            catch
            {
                return new OperationResult<bool>
                {
                    Success = false,
                    Message = "Erro ao deletar tipo de despesa",
                    Data = false
                };
            }
        }

    }
}