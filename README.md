# networkScript

A primitive scripting language I develop when I'm bored.

This doesn't aim at anything, I was just very bored


Inspired by [serenity's](https://github.com/SerenityOS/serenity/) [LibJS](https://github.com/SerenityOS/serenity/tree/master/Libraries/LibJS)

# Licence

All source code in the scope of this project is licenced under the Attribution-NonCommercial 4.0 International (CC BY-NC 4.0) Licence.
In summary you may distribute, modify and use this project as long as it is not with a commercial intend and as long as you give credit to the original source and author.
For more detail visit [the creative commons website](https://creativecommons.org/licenses/by-nc/4.0/).

# Example

```ts
for (let i = 0; i < 10; i++) {
    console.log('i is ' + i);
}
```
```
For(for) at line 0:0
ParenOpen(() at line 0:4
Let(let) at line 0:5
Identifier(i) at line 0:9
Equals(=) at line 0:11
NumericLiteral(0) at line 0:13
Semicolon(;) at line 0:14
Identifier(i) at line 0:16
Less(<) at line 0:18
NumericLiteral(10) at line 0:20
Semicolon(;) at line 0:22
Identifier(i) at line 0:24
PlusPlus(++) at line 0:25
ParenClose()) at line 0:27
CurlyOpen({) at line 0:29
Identifier(console) at line 1:4
Period(.) at line 1:11
Identifier(log) at line 1:12
ParenOpen(() at line 1:15
StringLiteral(i is ) at line 1:16
Plus(+) at line 1:24
Identifier(i) at line 1:26
ParenClose()) at line 1:27
Semicolon(;) at line 1:28
CurlyClose(}) at line 2:0

Tokenized source in 71 ms (714014 ticks)

BlockStatement
  ForStatement
    BinaryExpression
      VariableDeclaration
        IdentifierList
          Identifier(i)
      Reference
      Value(0)
    BinaryExpression
      Identifier(i)
      Less
      Value(10)
    PostfixExpression
      Increment
      Identifier(i)
    BlockStatement
      ExpressionStatement
        CallExpression
          MemberExpression
            Identifier(console)
            Identifier(log)
          BinaryExpression
            Value("i is ")
            Add
            Identifier(i)

Parsed tokens in 3 ms (36030 ticks)

i is 0
i is 1
i is 2
i is 3
i is 4
i is 5
i is 6
i is 7
i is 8
i is 9

Interpreted tree in 8 ms (85589 ticks)
```