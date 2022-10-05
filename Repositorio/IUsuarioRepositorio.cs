﻿using ControleDeContatos.Models;

namespace ControleDeContatos.Repositorio
{
    public interface IUsuarioRepositorio
    {
        List<UsuarioModel> BuscarTodos();
        UsuarioModel ListarPorId(int id);
        UsuarioModel Adicionar(UsuarioModel usuario);
        UsuarioModel Atualizar(UsuarioModel usuario);
        bool Apagar(int id);
        UsuarioModel BuscarPorLogin(string login);
        UsuarioModel BuscarPorEmailELogin(string email, string login);
        UsuarioModel AlterarSenha(AlterarSenhaModel alterarSenhaModel);

    }
}
