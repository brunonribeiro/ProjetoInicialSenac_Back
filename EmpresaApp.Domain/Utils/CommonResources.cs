namespace EmpresaApp.Domain.Utils
{
    public class CommonResources
    {
        public const string MsgCampoObrigatorio = "O campo {0} é obrigatório.";
        public const string MsgCampoInvalido = "O campo {0} está inválido.";
        public const string MsgDominioComMesmoNomeNoMasculino = "Já existe um {0} com mesmo nome.";
        public const string MsgDominioComMesmoNomeNoFeminino = "Já existe uma {0} com mesmo nome.";
        public const string MsgDominioNaoCadastradoNoMasculino = "O {0} informado não está cadastrado.";
        public const string MsgDominioNaoCadastradoNoFeminino = "A {0} informada não está cadastrada.";

        public const string CargoDominio = "Cargo";
        public const string EmpresaDominio = "Empresa";
        public const string FuncionarioDominio = "Funcionário";

        public const string OrdemDesc = "desc";

        #region Cargo
        public const string MsgCargoEstaVinculadoComFuncionario = "O cargo não pode ser excluído pois está vinculado com um funcionário.";
        #endregion

        #region Empresa
        public const string MsgEmpresaEstaVinculadoComFuncionario = "A empresa não pode ser excluída pois está vinculada com um funcionário.";
        #endregion

        #region Funcionario
        public const string MsgFuncionarioNaoPossuiEmpresaVinculada = "O funcionário precisa estar vinculado a uma empresa para atribuir um cargo.";
        #endregion

    }
}
