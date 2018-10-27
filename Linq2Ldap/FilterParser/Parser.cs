using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Linq2Ldap.ExtensionMethods;
using Linq2Ldap.Models;

namespace Linq2Ldap.FilterParser
{
    public class Parser
    {
        protected ILexer Lexer { get; set; }
        public Parser(ILexer lexer = null) {
            Lexer = lexer ?? new Lexer();
        }

        public Expression<Func<T, bool>> Parse<T>(string filter)
            where T: IEntry
        {
            var tokens = Lexer.Lex(filter);
            int startPos = 0, endPos = tokens.Count() - 1;
            var argParam = Expression.Parameter(typeof(T), "m");
            var body = _Parse<T>(tokens, startPos, endPos, argParam);
            return Expression.Lambda<Func<T, bool>>(body, argParam);
        }

        internal Expression _Parse<T>(
            IEnumerable<Token> tokens,
            int startPos,
            int endPos,
            ParameterExpression paramExpr
        )
            where T: IEntry
        {
            var start = tokens.ElementAt(startPos);
            var end = tokens.ElementAt(endPos);
            if (start.Text != Tokens.LeftParen || end.Text != Tokens.RightParen) {
                throw new SyntaxException("Filters are Lisp-like and must begin and end with parentheses.", startPos, endPos);
            }

            return ParseUnguarded<T>(tokens, startPos + 1, endPos - 1, paramExpr);
        }

        internal Expression ParseUnguarded<T>(
            IEnumerable<Token> tokens,
            int startPos,
            int endPos,
            ParameterExpression paramExpr
        )
            where T: IEntry
        {
            var op = tokens.ElementAt(startPos);
            if (new [] { Tokens.And, Tokens.Or }.Contains(op.Text)) {
                return CreateListOp<T>(op, tokens, startPos + 1, endPos, paramExpr);
            }

            if (Tokens.Not == op.Text) {
                return CreateNegation<T>(tokens, startPos + 1, endPos, paramExpr);
            }

            int len = endPos - startPos + 1;
            if (len == 2 && tokens.ElementAt(endPos).Text == Tokens.Present) {
                return CreatePresenceCheck<T>(op, tokens, startPos, endPos, paramExpr);
            }

            if (len == 3) {
                return CreateSimpleCompare<T>(tokens, startPos, endPos, paramExpr);
            }

            if (len > 3 && tokens.ElementAt(startPos + 1).Text == Tokens.Equal) {
                return CreateMatchCheck<T>(op, tokens, startPos + 2, endPos, paramExpr);
            }

            throw new SyntaxException($"Unrecognized expression type.", startPos, endPos);
        }

        internal Expression CreatePresenceCheck<T>(
            Token left,
            IEnumerable<Token> tokens,
            int startPos,
            int endPos,
            ParameterExpression paramExpr
        )
            where T: IEntry
        {
            if (left.IsDefinedSymbol) {
                var memberRef = BuildPropertyExpr<T>(left, paramExpr);
                var methodInfo = typeof(PropertyExtensions).GetMethod(nameof(PropertyExtensions.Matches));
                return Expression.Call(methodInfo, memberRef, Expression.Constant("*"));
            }

            throw new SyntaxException($"Unrecognized token type in presence check: {left.Text}.", left.StartPos, left.EndPos);
        }

        internal Expression CreateMatchCheck<T>(
            Token left,
            IEnumerable<Token> tokens,
            int startPos,
            int endPos,
            ParameterExpression paramExpr
        )
            where T: IEntry
        {
            if (! left.IsDefinedSymbol) {
                var memberRef = BuildPropertyExpr<T>(left, paramExpr);
                var methodInfo = typeof(PropertyExtensions).GetMethod(nameof(PropertyExtensions.Matches));
                var matchAgg = tokens
                    .Skip(startPos)
                    .Take(endPos - startPos + 1)
                    .Aggregate(
                        new Token("", 0, tokens.ElementAt(startPos).IsDefinedSymbol),
                        AggregateOneToken);
                return Expression.Call(methodInfo, memberRef, Expression.Constant(matchAgg.Text));
            }

            throw new SyntaxException($"Invalid member reference in presence check: {left.Text}.", left.StartPos, left.EndPos);
        }

        internal Token AggregateOneToken(Token acc, Token cur) {
            if (! cur.IsDefinedSymbol || cur.Text != Tokens.Star) {
                throw new SyntaxException($"This symbol not allowed in match expression: {cur.Text}.", cur.StartPos, cur.EndPos);
            }

            if (cur.IsDefinedSymbol != acc.IsDefinedSymbol) {
                acc.Text += cur.Text;
                acc.IsDefinedSymbol = cur.IsDefinedSymbol;
            }

            return acc;
        }

        internal Expression CreateSimpleCompare<T>(
            IEnumerable<Token> tokens,
            int startPos,
            int endPos,
            ParameterExpression paramExpr
        )
            where T: IEntry
        {
            var left = tokens.ElementAt(startPos);
            var right = tokens.ElementAt(startPos + 2);
            if (right.IsDefinedSymbol || left.IsDefinedSymbol) {
                throw new SyntaxException(
                    $"Binary expression arguments cannot include operators (except asterisks on the right-hand side).",
                    right.StartPos, right.EndPos);
            }

            var memberRef = BuildPropertyExpr<T>(left, paramExpr);
            var op = tokens.ElementAt(startPos + 1);
            switch(op.Text) {
                case Tokens.Equal:
                    return Expression.Equal(memberRef, Expression.Constant(right.Text));
                case Tokens.Present:
                    return Expression.Equal(memberRef, Expression.Constant($"*{right.Text}"));
                case Tokens.GTE:
                    return Expression.GreaterThanOrEqual(memberRef, Expression.Constant(right.Text));
                case Tokens.LTE:
                    return Expression.LessThanOrEqual(memberRef, Expression.Constant(right.Text));
                case Tokens.Approx:
                    var methodInfo = typeof(PropertyExtensions).GetMethod(nameof(PropertyExtensions.Approx));
                    return Expression.Call(methodInfo, memberRef, Expression.Constant(right.Text));
                default:
                    throw new SyntaxException($"Unrecognized operator: {op.Text}.", op.StartPos, op.EndPos);
            }
        }

        internal Expression BuildPropertyExpr<T>(
            Token left,
            ParameterExpression paramExpr
        )
            where T: IEntry
        {
            // TODO: If a field exists dedicated to this property access, use that, instead.
            return Expression.Property(paramExpr, "Item", Expression.Constant(left.Text));
        }

        internal Expression CreateListOp<T>(
            Token op,
            IEnumerable<Token> tokens,
            int startPos,
            int endPos,
            ParameterExpression paramExpr
        )
            where T: IEntry
        {
            if (startPos == endPos) {
                return null;
            }

            var paren = tokens.ElementAt(startPos);
            if (paren.Text != Tokens.LeftParen) {
                throw new SyntaxException($"Sub-filter must begin with left paren. Was: {paren.Text}.", startPos, endPos);
            }

            // Find the matching right parent at our nesting depth.
            int nextEnd = -1, depth = 0;
            for (var i = startPos + 1; i < endPos; i++) {
                var tok = tokens.ElementAt(i);
                if (tok.Text == Tokens.RightParen)
                {
                    if (depth == 0) {
                        nextEnd = i;
                        break;
                    }

                    depth = depth - 1;
                } else if (tok.Text == Tokens.LeftParen) {
                    depth = depth + 1;
                }
            }
            
            if (nextEnd == -1) {
                throw new SyntaxException($"Mismatched left paren.", startPos, endPos);
            }

            var subExpr = ParseUnguarded<T>(tokens, startPos, nextEnd, paramExpr);
            if (nextEnd != endPos) {
                var subExpr2 = CreateListOp<T>(op, tokens, nextEnd + 1, endPos, paramExpr);
                return Expression.AndAlso(subExpr, subExpr2);
            }

            return subExpr;
        }

        internal Expression CreateNegation<T>(
            IEnumerable<Token> tokens,
            int startPos,
            int endPos,
            ParameterExpression paramExpr
        )
            where T: IEntry
        {
            return Expression.Not(_Parse<T>(tokens, startPos, endPos, paramExpr));
        }
    }
}