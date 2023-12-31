// DO NOT EDIT: Autogenerated file created on 2023-10-13 4:32:00 PM. 
using System;
using System.Linq;
using System.Collections.Generic;

namespace Parakeet.Demos.Plato
{
    public class CstNodeFactory
    {
        public static Parakeet.Demos.PlatoGrammar Grammar = new Parakeet.Demos.PlatoGrammar();
        public Dictionary<CstNode, ParserTreeNode> Lookup { get;} = new Dictionary<CstNode, ParserTreeNode>();
        public CstNode Create(ParserTreeNode node)
        {
            var r = InternalCreate(node);
            Lookup.Add(r, node);
            return r;
        }
        public CstNode InternalCreate(ParserTreeNode node)
        {
            switch (node.Type)
            {
                case "CastExpression": return new CstCastExpression(node.Children.Select(Create).ToArray());
                case "PrefixOperator": return new CstPrefixOperator(node.Contents);
                case "Indexer": return new CstIndexer(node.Children.Select(Create).ToArray());
                case "PostfixOperator": return new CstPostfixOperator(node.Children.Select(Create).ToArray());
                case "Identifier": return new CstIdentifier(node.Contents);
                case "BinaryOperation": return new CstBinaryOperation(node.Children.Select(Create).ToArray());
                case "TernaryOperation": return new CstTernaryOperation(node.Children.Select(Create).ToArray());
                case "ParenthesizedExpression": return new CstParenthesizedExpression(node.Children.Select(Create).ToArray());
                case "ThrowExpression": return new CstThrowExpression(node.Children.Select(Create).ToArray());
                case "LambdaParameter": return new CstLambdaParameter(node.Children.Select(Create).ToArray());
                case "LambdaParameters": return new CstLambdaParameters(node.Children.Select(Create).ToArray());
                case "LambdaBody": return new CstLambdaBody(node.Children.Select(Create).ToArray());
                case "LambdaExpr": return new CstLambdaExpr(node.Children.Select(Create).ToArray());
                case "MemberAccess": return new CstMemberAccess(node.Children.Select(Create).ToArray());
                case "ConditionalMemberAccess": return new CstConditionalMemberAccess(node.Children.Select(Create).ToArray());
                case "TypeOf": return new CstTypeOf(node.Children.Select(Create).ToArray());
                case "NameOf": return new CstNameOf(node.Children.Select(Create).ToArray());
                case "Default": return new CstDefault(node.Children.Select(Create).ToArray());
                case "InitializerClause": return new CstInitializerClause(node.Children.Select(Create).ToArray());
                case "Initializer": return new CstInitializer(node.Children.Select(Create).ToArray());
                case "ArraySizeSpecifier": return new CstArraySizeSpecifier(node.Children.Select(Create).ToArray());
                case "NewOperation": return new CstNewOperation(node.Children.Select(Create).ToArray());
                case "IsOperation": return new CstIsOperation(node.Children.Select(Create).ToArray());
                case "AsOperation": return new CstAsOperation(node.Children.Select(Create).ToArray());
                case "StringInterpolationContent": return new CstStringInterpolationContent(node.Children.Select(Create).ToArray());
                case "StringInterpolation": return new CstStringInterpolation(node.Children.Select(Create).ToArray());
                case "FunctionArgKeyword": return new CstFunctionArgKeyword(node.Contents);
                case "FunctionArg": return new CstFunctionArg(node.Children.Select(Create).ToArray());
                case "FunctionArgs": return new CstFunctionArgs(node.Children.Select(Create).ToArray());
                case "LeafExpression": return new CstLeafExpression(node.Children.Select(Create).ToArray());
                case "Expression": return new CstExpression(node.Children.Select(Create).ToArray());
                case "ExpressionStatement": return new CstExpressionStatement(node.Children.Select(Create).ToArray());
                case "ElseClause": return new CstElseClause(node.Children.Select(Create).ToArray());
                case "IfStatement": return new CstIfStatement(node.Children.Select(Create).ToArray());
                case "WhileStatement": return new CstWhileStatement(node.Children.Select(Create).ToArray());
                case "DoWhileStatement": return new CstDoWhileStatement(node.Children.Select(Create).ToArray());
                case "ReturnStatement": return new CstReturnStatement(node.Children.Select(Create).ToArray());
                case "BreakStatement": return new CstBreakStatement(node.Contents);
                case "YieldStatement": return new CstYieldStatement(node.Children.Select(Create).ToArray());
                case "YieldReturn": return new CstYieldReturn(node.Children.Select(Create).ToArray());
                case "YieldBreak": return new CstYieldBreak(node.Contents);
                case "ContinueStatement": return new CstContinueStatement(node.Contents);
                case "CompoundStatement": return new CstCompoundStatement(node.Children.Select(Create).ToArray());
                case "CatchClause": return new CstCatchClause(node.Children.Select(Create).ToArray());
                case "FinallyClause": return new CstFinallyClause(node.Children.Select(Create).ToArray());
                case "CaseClause": return new CstCaseClause(node.Children.Select(Create).ToArray());
                case "SwitchStatement": return new CstSwitchStatement(node.Children.Select(Create).ToArray());
                case "TryStatement": return new CstTryStatement(node.Children.Select(Create).ToArray());
                case "ForEachStatement": return new CstForEachStatement(node.Children.Select(Create).ToArray());
                case "FunctionDeclStatement": return new CstFunctionDeclStatement(node.Children.Select(Create).ToArray());
                case "ForLoopInit": return new CstForLoopInit(node.Children.Select(Create).ToArray());
                case "ForLoopInvariant": return new CstForLoopInvariant(node.Children.Select(Create).ToArray());
                case "ForLoopVariant": return new CstForLoopVariant(node.Children.Select(Create).ToArray());
                case "ForStatement": return new CstForStatement(node.Children.Select(Create).ToArray());
                case "ArrayInitializationValue": return new CstArrayInitializationValue(node.Children.Select(Create).ToArray());
                case "InitializationValue": return new CstInitializationValue(node.Children.Select(Create).ToArray());
                case "Initialization": return new CstInitialization(node.Children.Select(Create).ToArray());
                case "VarWithInitialization": return new CstVarWithInitialization(node.Children.Select(Create).ToArray());
                case "VarDecl": return new CstVarDecl(node.Children.Select(Create).ToArray());
                case "VarDeclStatement": return new CstVarDeclStatement(node.Children.Select(Create).ToArray());
                case "Statement": return new CstStatement(node.Children.Select(Create).ToArray());
                case "QualifiedIdentifier": return new CstQualifiedIdentifier(node.Children.Select(Create).ToArray());
                case "TypeParameter": return new CstTypeParameter(node.Children.Select(Create).ToArray());
                case "TypeParameterList": return new CstTypeParameterList(node.Children.Select(Create).ToArray());
                case "ImplementsList": return new CstImplementsList(node.Children.Select(Create).ToArray());
                case "InheritsList": return new CstInheritsList(node.Children.Select(Create).ToArray());
                case "Type": return new CstType(node.Children.Select(Create).ToArray());
                case "Concept": return new CstConcept(node.Children.Select(Create).ToArray());
                case "Library": return new CstLibrary(node.Children.Select(Create).ToArray());
                case "TopLevelDeclaration": return new CstTopLevelDeclaration(node.Children.Select(Create).ToArray());
                case "FunctionParameter": return new CstFunctionParameter(node.Children.Select(Create).ToArray());
                case "FunctionParameterList": return new CstFunctionParameterList(node.Children.Select(Create).ToArray());
                case "ExpressionBody": return new CstExpressionBody(node.Children.Select(Create).ToArray());
                case "FunctionBody": return new CstFunctionBody(node.Children.Select(Create).ToArray());
                case "TypeAnnotation": return new CstTypeAnnotation(node.Children.Select(Create).ToArray());
                case "MethodDeclaration": return new CstMethodDeclaration(node.Children.Select(Create).ToArray());
                case "FieldDeclaration": return new CstFieldDeclaration(node.Children.Select(Create).ToArray());
                case "MemberDeclaration": return new CstMemberDeclaration(node.Children.Select(Create).ToArray());
                case "ArrayRankSpecifier": return new CstArrayRankSpecifier(node.Contents);
                case "ArrayRankSpecifiers": return new CstArrayRankSpecifiers(node.Children.Select(Create).ToArray());
                case "TypeArgList": return new CstTypeArgList(node.Children.Select(Create).ToArray());
                case "SimpleTypExpr": return new CstSimpleTypExpr(node.Children.Select(Create).ToArray());
                case "CompoundTypeExpr": return new CstCompoundTypeExpr(node.Children.Select(Create).ToArray());
                case "CompoundOrSimpleTypeExpr": return new CstCompoundOrSimpleTypeExpr(node.Children.Select(Create).ToArray());
                case "InnerTypeExpr": return new CstInnerTypeExpr(node.Children.Select(Create).ToArray());
                case "TypeExpr": return new CstTypeExpr(node.Children.Select(Create).ToArray());
                case "File": return new CstFile(node.Children.Select(Create).ToArray());
                case "FloatLiteral": return new CstFloatLiteral(node.Contents);
                case "HexLiteral": return new CstHexLiteral(node.Contents);
                case "BinaryLiteral": return new CstBinaryLiteral(node.Contents);
                case "IntegerLiteral": return new CstIntegerLiteral(node.Contents);
                case "StringLiteral": return new CstStringLiteral(node.Contents);
                case "CharLiteral": return new CstCharLiteral(node.Contents);
                case "BooleanLiteral": return new CstBooleanLiteral(node.Contents);
                case "NullLiteral": return new CstNullLiteral(node.Contents);
                case "Literal": return new CstLiteral(node.Children.Select(Create).ToArray());
                case "BinaryOperator": return new CstBinaryOperator(node.Contents);
                case "Operator": return new CstOperator(node.Contents);
                case "Separator": return new CstSeparator(node.Children.Select(Create).ToArray());
                case "Delimiter": return new CstDelimiter(node.Contents);
                case "TypeKeyword": return new CstTypeKeyword(node.Contents);
                case "StatementKeyword": return new CstStatementKeyword(node.Contents);
                case "Unknown": return new CstUnknown(node.Contents);
                case "ParameterName": return new CstParameterName(node.Children.Select(Create).ToArray());
                case "FunctionName": return new CstFunctionName(node.Children.Select(Create).ToArray());
                case "FieldName": return new CstFieldName(node.Children.Select(Create).ToArray());
                case "TypeName": return new CstTypeName(node.Children.Select(Create).ToArray());
                case "TypeParameterToken": return new CstTypeParameterToken(node.Children.Select(Create).ToArray());
                case "TypeParametersToken": return new CstTypeParametersToken(node.Children.Select(Create).ToArray());
                case "Comment": return new CstComment(node.Contents);
                default: throw new Exception($"Unrecognized parse node {node.Type}");
            }
        }
    }
}
