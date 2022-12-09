﻿using Sidl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidl.Processor {
  public class Linter {

    public SidlParser Parser { 
      get { return _parser; } 
      set { _parser = value; } 
    }

    private SidlParser _parser;

    public Linter(SidlParser parser) {
      _parser = parser; 
    }

    public ScopedSymbolTable CreateScopedSymbolTable() {
      var scopedSymbolTableVisitor = new ScopedSymbolTableVisitor();
      SidlParser.RootContext rootContext = _parser.root();
      try {
        scopedSymbolTableVisitor.Visit(rootContext);
      } catch (Exception e) {
        Console.WriteLine("\n !!! Parser Exception: " + e.Message);
      }

      var scopedSymbolTable = scopedSymbolTableVisitor.scopedSymbolTable;
      return scopedSymbolTable;
    }


    [Obsolete("Method is deprecated due to the new scoped symbol table implementation and hence, will be removed soon.")]
    public void CompleteScopeSymbolDependencies(Dictionary<Scope, List<Symbol>> scopeSymbolStore) {
      foreach (var scope in scopeSymbolStore.Keys.Where(x => x.Parent != null)) {
        scope.Parent.ChildScopes.Add(scope.Name, scope);        
      }
    }

    [Obsolete("Method is deprecated due to the new scoped symbol table implementation and hence, will be removed soon.")]
    public Dictionary<Scope, Dictionary<string, int>> CheckScopeSymbolDuplicates(Dictionary<Scope, List<Symbol>> scopeSymbolStore) {
      // check uniqueness of each declaration per scope (allow shadowing)
      var scopeSymbolDuplicateStore = new Dictionary<Scope, Dictionary<string, int>>();
      foreach (var scopedDecls in scopeSymbolStore) {
        scopeSymbolDuplicateStore.Add(scopedDecls.Key, new Dictionary<string, int>());
        foreach(var declaration in scopedDecls.Value) {
          if (scopeSymbolDuplicateStore[scopedDecls.Key].ContainsKey(declaration.Name)) scopeSymbolDuplicateStore[scopedDecls.Key][declaration.Name]++;
          else scopeSymbolDuplicateStore[scopedDecls.Key].Add(declaration.Name, 1);
        }
        foreach(var item in scopeSymbolDuplicateStore[scopedDecls.Key].Where(x => x.Value == 1).ToList()) {
          scopeSymbolDuplicateStore[scopedDecls.Key].Remove(item.Key);
        }
      }
      return scopeSymbolDuplicateStore;
    }


    [Obsolete("Method is deprecated due to the new scoped symbol table implementation and hence, will be removed soon.")]
    public static void PrintScopeSymbolStore(Dictionary<Scope, List<Symbol>> scopeSymbolStore, Scope currentScope, int scopeLevel) {
      string indent = "  ";
      for (int i = 0; i < scopeLevel; i++) indent += "  ";

      Console.WriteLine($"\n{indent}scope l{scopeLevel}: {currentScope}");
      foreach (var declaration in scopeSymbolStore[currentScope]) {
        Console.WriteLine(indent + declaration);
      }

      foreach (var kvp in scopeSymbolStore.Where(x => x.Key.Parent == currentScope)) {
        PrintScopeSymbolStore(scopeSymbolStore, kvp.Key, scopeLevel + 1);
      }
    }

    [Obsolete("Method is deprecated due to the new scoped symbol table implementation and hence, will be removed soon.")]
    public static void PrintScopeSymbolStore(Dictionary<Scope, Dictionary<string, int>> scopeSymbolStore, Scope currentScope, int scopeLevel) {
      string indent = "  ";
      for (int i = 0; i < scopeLevel; i++) indent += "  ";

      Console.WriteLine($"\n{indent}scope l{scopeLevel}: {currentScope}");
      foreach (var declaration in scopeSymbolStore[currentScope]) {
        Console.WriteLine($"{indent}{declaration.Key}: {declaration.Value}");
      }

      foreach (var kvp in scopeSymbolStore.Where(x => x.Key.Parent == currentScope)) {
        PrintScopeSymbolStore(scopeSymbolStore, kvp.Key, scopeLevel + 1);
      }
    }
    
    public static void PrintProgramFormatted(string programText) {
      Console.WriteLine(programText);
    }

  }
}
