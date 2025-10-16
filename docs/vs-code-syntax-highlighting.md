## Project Creation and Management

### Extension Location

VS Code extensions are installed in: `C:\Users\[USER]\.vscode\extensions` (Windows) or `~/.vscode/extensions` (Mac/Linux)

### Creating a Syntax Highlighting Extension

1. **Install Yeoman and the VS Code Extension Generator**
   ```bash
   npm install -g yo generator-code
   ```

2. **Generate the Extension Scaffold**
   ```bash
   yo code
   ```
   - Select "New Language Support"
   - Answer the prompts:
     - Language name (e.g., "Qik")
     - Language identifier (e.g., "qik")
     - File extensions (e.g., ".qik")
     - Scope name (e.g., "source.qik")

3. **Project Structure**
   The generator creates:
   - `package.json` - Extension manifest with language configuration
   - `syntaxes/[language].tmLanguage.json` - TextMate grammar file (this is where you define syntax rules)
   - `language-configuration.json` - Language-specific features (brackets, comments, etc.)

4. **Develop Your Grammar**
   - Edit `syntaxes/[language].tmLanguage.json` to define syntax patterns
   - Requires knowledge of:
     - Regular expressions (Oniguruma flavor, not standard JavaScript regex)
     - TextMate scope naming conventions
   - Use the **Scope Inspector** (`Ctrl+Shift+P` → "Developer: Inspect Editor Tokens and Scopes") to debug

5. **Testing During Development**
   - Press `F5` in VS Code to open an Extension Development Host window
   - Test your syntax highlighting in real-time
   - Make changes and reload to see updates

6. **Install Locally**
   Once satisfied:
   - Copy the entire extension folder to `C:\Users\[USER]\.vscode\extensions\[extension-name]`
   - Restart VS Code
   - Verify syntax highlighting works for files with your specified extension

7. **Publishing (Optional)**
   To share with others:
   ```bash
   npm install -g @vscode/vsce
   vsce package
   ```
   This creates a `.vsix` file you can share or publish to the VS Code Marketplace.

## References

### Official Documentation
- [VS Code Syntax Highlight Guide](https://code.visualstudio.com/api/language-extensions/syntax-highlight-guide) - Official guide from Microsoft
- [Language Configuration Guide](https://code.visualstudio.com/api/language-extensions/language-configuration-guide) - Brackets, comments, auto-closing pairs
- [VS Code Extension API](https://code.visualstudio.com/api) - Complete extension development documentation
- [Color Theme Guide](https://code.visualstudio.com/api/extension-guides/color-theme#syntax-colors) - How themes apply colors to scopes

### TextMate Grammar Resources
- [TextMate Language Grammars](https://macromates.com/manual/en/language_grammars) - Comprehensive scope naming conventions
- [Writing a TextMate Grammar - Lessons Learned](https://www.apeth.com/nonblog/stories/textmatebundle.html) - Practical guide with examples
- [TextMate Grammar Guide (GitHub Gist)](https://gist.github.com/Aerijo/b8c82d647db783187804e86fa0a604a1) - Detailed technical reference

### Regular Expressions
- [Oniguruma Regular Expressions](https://github.com/kkos/oniguruma/blob/master/doc/RE) - The regex engine used by TextMate grammars
- [TextMate Regex Guide](https://macromates.com/manual/en/regular_expressions) - Regex in the TextMate context
- [Rubular](https://rubular.com/) - Online regex tester (note: uses Ruby regex, which is similar but not identical to Oniguruma)

### Development Tools
- **Scope Inspector**: Press `Ctrl+Shift+P` (Windows/Linux) or `Cmd+Shift+P` (Mac) → "Developer: Inspect Editor Tokens and Scopes"
  - Shows the scope chain for any token in your editor
  - Essential for debugging syntax highlighting

### Video Tutorials
- [Create Custom Syntax Highlighting in VS Code](https://www.youtube.com/watch?v=5msZv-nKebI) - Complete walkthrough
  - [3:06](https://youtu.be/5msZv-nKebI?t=186) - Creating template project with Yeoman
  - [6:27](https://youtu.be/5msZv-nKebI?t=387) - Configuring `package.json`
  - [11:39](https://youtu.be/5msZv-nKebI?t=699) - Installing to VS Code extensions directory

### Additional Resources
- [TextMate: Power Editing for the Mac](https://www.amazon.com/gp/product/097873923X/ref=dbs_a_def_rwt_bibl_vppi_i1) - Book covering TextMate grammars in depth

### Tips
- Start with an existing grammar similar to your language as a reference
- Use the Scope Inspector frequently while developing
- Test with the Extension Development Host (`F5`) for immediate feedback
- Keep scope names consistent with TextMate conventions for better theme compatibility
