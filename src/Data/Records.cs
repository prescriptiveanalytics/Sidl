﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ai.Hgb.Seidl.Data {

  public record NodetypesRequest(string programText, int line, int character);
  public record ProgramRecord(string programText);
  public record NodeRecord(string name);
  public record EdgeRecord(string name, string from, string to, string type, string payload);
  public record GraphRecord(IEnumerable<NodeRecord> nodes, IEnumerable<EdgeRecord> edges);

}
